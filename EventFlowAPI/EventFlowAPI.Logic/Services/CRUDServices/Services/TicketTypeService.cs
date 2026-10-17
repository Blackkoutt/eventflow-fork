using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class TicketTypeService(IUnitOfWork unitOfWork, IAuthService authService) :
        GenericService<
            TicketType,
            TicketTypeRequestDto,
            UpdateTicketTypeRequestDto,
            TicketTypeResponseDto,
            TicketTypeQuery
        >(unitOfWork, authService),
        ITicketTypeService
    {
        public sealed override async Task<Result<IEnumerable<TicketTypeResponseDto>>> GetAllAsync(TicketTypeQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.Where(s => !s.IsDeleted && !s.IsSoftUpdated)
                                                              .ByName(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<TicketTypeResponseDto>>.Success(response);
        }

        public sealed override async Task<Result<TicketTypeResponseDto>> UpdateAsync(int id, UpdateTicketTypeRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto, id);
            if (error != Error.None)
                return Result<TicketTypeResponseDto>.Failure(error);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<TicketTypeResponseDto>.Failure(Error.NotFound);

            var newEntity = oldEntity;
            if (oldEntity is ISoftUpdateable softUpdateable && oldEntity.Tickets.Any(t => t.Reservations.Any()))
            {
                var entity = MapUpdatedAsEntity(requestDto!);
                var ticketsWithoutReservations = oldEntity.Tickets.Where(t => !t.Reservations.Any());
                entity.Tickets = ticketsWithoutReservations.ToList();
                await _repository.AddAsync(entity);
                ((ISoftUpdateable)newEntity).IsSoftUpdated = true;
            }
            newEntity = (TicketType)MapToEntity(requestDto!, oldEntity);
            var ticketsWithReservations = oldEntity.Tickets.Where(t => t.Reservations.Any()).ToList();
            newEntity.Tickets = ticketsWithReservations;
            _repository.Update(newEntity);

            await _unitOfWork.SaveChangesAsync();
            
            return Result<TicketTypeResponseDto>.Success();
        }

        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            await DeleteEntity(entity, isSoftDelete: entity.Tickets.Any(t => t.Reservations.Any()));

            return Result<object>.Success();
        }

        protected sealed override async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            if (id != null && id < 0)
                return Error.RouteParamOutOfRange;

            if (requestDto == null)
                return Error.NullParameter;

            var isSameEntityExistsResult = await IsSameEntityExistInDatabase(requestDto, id);
            if (!isSameEntityExistsResult.IsSuccessful) return isSameEntityExistsResult.Error;

            var isSameEntityExistInDb = isSameEntityExistsResult.Value;
            if (isSameEntityExistInDb)
                return Error.SuchEntityExistInDb;

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return userResult.Error;

            var user = userResult.Value;
            if (!user.IsInRole(Roles.Admin))
                return AuthError.UserDoesNotHavePremissionToResource;

            if (id != null)
            {
                var entity = await _repository.GetOneAsync((int)id);
                if (entity == null || entity.IsDeleted || entity.IsSoftUpdated)
                    return Error.NotFound;
            }

            return Error.None;
        }

        protected sealed override async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower() &&
                        !entity.IsDeleted &&
                        !entity.IsSoftUpdated
                      ))).Any();

            return Result<bool>.Success(result);
        }

        protected override TicketTypeResponseDto MapResponse(TicketType entity) => entity.ToResponse();

        protected override TicketType MapAsEntity(TicketTypeRequestDto requestDto) => requestDto.ToEntity();

        protected override TicketType MapUpdatedAsEntity(UpdateTicketTypeRequestDto requestDto) => requestDto.ToEntity();

        protected override IEntity MapToEntity(UpdateTicketTypeRequestDto requestDto, TicketType oldEntity) => requestDto.ApplyTo(oldEntity);
    }
}
