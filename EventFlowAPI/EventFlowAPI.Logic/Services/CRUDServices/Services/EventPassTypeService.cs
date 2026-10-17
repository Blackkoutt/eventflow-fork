using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.Logic.UnitOfWork;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class EventPassTypeService(IUnitOfWork unitOfWork, IAuthService authService) :
        GenericService<
            EventPassType,
            EventPassTypeRequestDto,
            UpdateEventPassTypeRequestDto,
            EventPassTypeResponseDto,
            EventPassTypeQuery
        >(unitOfWork, authService),
        IEventPassTypeService
    {
        public sealed override async Task<Result<IEnumerable<EventPassTypeResponseDto>>> GetAllAsync(EventPassTypeQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.Where(ept => !ept.IsDeleted)
                                                              .ByName(query)
                                                              .ByPrice(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<EventPassTypeResponseDto>>.Success(response);
        }

        public sealed override async Task<Result<EventPassTypeResponseDto>> UpdateAsync(int id, UpdateEventPassTypeRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto, id);
            if (error != Error.None)
                return Result<EventPassTypeResponseDto>.Failure(error);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<EventPassTypeResponseDto>.Failure(Error.NotFound);

            await UpdateEntity(oldEntity, requestDto!);

            return Result<EventPassTypeResponseDto>.Success();
        }

        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            await DeleteEntity(entity, isSoftDelete: entity.EventPasses.Any());

            return Result<object>.Success();
        }

        protected sealed override async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            if (id != null && id < 0)
                return Error.RouteParamOutOfRange;

            if (requestDto == null)
                return Error.NullParameter;

            if (id == null)
            {
                var isSameEntityExistsResult = await IsSameEntityExistInDatabase(requestDto, id);
                if (!isSameEntityExistsResult.IsSuccessful) return isSameEntityExistsResult.Error;

                var isSameEntityExistInDb = isSameEntityExistsResult.Value;
                if (isSameEntityExistInDb)
                    return Error.SuchEntityExistInDb;
            }

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

        protected async sealed override Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            if (requestDto is not INameableRequestDto) return Result<bool>.Failure(Error.BadRequestType);

            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower() &&
                        !entity.IsDeleted
                      ))).Any();

            return Result<bool>.Success(result);
        }

        protected override EventPassTypeResponseDto MapAsDto(EventPassType entity) => entity.ToResponse();

        protected override EventPassType MapAsEntity(EventPassTypeRequestDto requestDto) => requestDto.ToEntity();

        protected override IEntity MapToEntity(UpdateEventPassTypeRequestDto requestDto, EventPassType oldEntity) => requestDto.ApplyTo(oldEntity);
    }
}
