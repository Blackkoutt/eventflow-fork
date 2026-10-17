using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
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
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class TicketTypeService(IUnitOfWork unitOfWork, IAuthService authService) :
        ITicketTypeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;
        private readonly IGenericRepository<TicketType> _repository = unitOfWork.GetRepository<TicketType>();

        public async Task<Result<IEnumerable<TicketTypeResponseDto>>> GetAllAsync(TicketTypeQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.Where(s => !s.IsDeleted)
                                                              .ByName(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<TicketTypeResponseDto>>.Success(response);
        }

        public async Task<Result<TicketTypeResponseDto>> UpdateAsync(int id, UpdateTicketTypeRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto, id);
            if (error != Error.None)
                return Result<TicketTypeResponseDto>.Failure(error);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<TicketTypeResponseDto>.Failure(Error.NotFound);

            var newEntity = oldEntity;
            newEntity = (TicketType)MapToEntity(requestDto!, oldEntity);
            var ticketsWithReservations = oldEntity.Tickets.Where(t => t.Reservations.Any()).ToList();
            newEntity.Tickets = ticketsWithReservations;
            _repository.Update(newEntity);

            await _unitOfWork.SaveChangesAsync();
            
            return Result<TicketTypeResponseDto>.Success();
        }

        public async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            await DeleteEntity(entity, isSoftDelete: entity.Tickets.Any(t => t.Reservations.Any()));

            return Result<object>.Success();
        }

        private async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
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
                if (entity == null || entity.IsDeleted)
                    return Error.NotFound;
            }

            return Error.None;
        }

        private async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower() &&
                        !entity.IsDeleted
                      ))).Any();

            return Result<bool>.Success(result);
        }

        private TicketTypeResponseDto MapAsDto(TicketType entity) => entity.ToResponse();

        private TicketType MapAsEntity(TicketTypeRequestDto requestDto) => requestDto.ToEntity();

        private IEntity MapToEntity(UpdateTicketTypeRequestDto requestDto, TicketType oldEntity) => requestDto.ApplyTo(oldEntity);

        public async Task<Result<TicketTypeResponseDto>> GetOneAsync(int id)
        {
            if (id < 0)
                return Result<TicketTypeResponseDto>.Failure(Error.RouteParamOutOfRange);

            var record = await _repository.GetOneAsync(id);
            if (record == null)
                return Result<TicketTypeResponseDto>.Failure(Error.NotFound);

            var response = MapAsDto(record);

            return Result<TicketTypeResponseDto>.Success(response);
        }

        public async Task<Result<TicketTypeResponseDto>> AddAsync(TicketTypeRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto);
            if (error != Error.None)
                return Result<TicketTypeResponseDto>.Failure(error);

            var entity = MapAsEntity(requestDto!);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<TicketTypeResponseDto>.Success(response);
        }

        private async Task DeleteEntity(TicketType entity, bool isSoftDelete)
        {
            if (isSoftDelete)
            {
                entity.IsDeleted = true;
                entity.DeleteDate = DateTime.Now;
                _repository.Update(entity);
            }
            else
            {
                _repository.Delete(entity);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<Result<(TicketType Entity, UserResponseDto User)>> ValidateBeforeDelete(int id)
        {
            if (id < 0)
                return Result<(TicketType Entity, UserResponseDto User)>.Failure(Error.RouteParamOutOfRange);

            var entity = await _repository.GetOneAsync(id);
            if (entity == null)
                return Result<(TicketType Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<(TicketType Entity, UserResponseDto User)>.Failure(userResult.Error);

            if (entity.IsDeleted)
                return Result<(TicketType Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var user = userResult.Value;
            if (!user.IsInRole(Roles.Admin))
                return Result<(TicketType Entity, UserResponseDto User)>.Failure(user.IsInRole(Roles.User) ? AuthError.UserDoesNotHavePremissionToResource : AuthError.UserDoesNotHaveSpecificRole);

            return Result<(TicketType Entity, UserResponseDto User)>.Success((entity, user));
        }

        private IEnumerable<TicketTypeResponseDto> MapAsDto(IEnumerable<TicketType> records)
        {
            return records.Select(entity => MapAsDto(entity));
        }
    }
}
