using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.DB.Entities.Abstract;
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
using EventFlowAPI.Logic.Identity.Services.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class EquipmentService(
        IUnitOfWork unitOfWork, 
        IAuthService authService) :
        GenericService<
            Equipment,
            EquipmentRequestDto,
            UpdateEquipmentRequestDto,
            EquipmentResponseDto,
            EquipmentQuery
        >(unitOfWork, authService),
        IEquipmentService
    {
        public sealed override async Task<Result<IEnumerable<EquipmentResponseDto>>> GetAllAsync(EquipmentQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.ByName(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<EquipmentResponseDto>>.Success(response);
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
                if (entity == null) return Error.NotFound;
            }

            return Error.None;
        }


        protected async sealed override Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            if (requestDto is not INameableRequestDto) return Result<bool>.Failure(Error.BadRequestType);

            var result = (await _repository.GetAllAsync(q =>
                          q.Where(entity =>
                            entity.Id != id &&
                            entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower()))).Any();

            return Result<bool>.Success(result);
        }

        protected override EquipmentResponseDto MapResponse(Equipment entity) => entity.ToResponse();

        protected override Equipment MapAsEntity(EquipmentRequestDto requestDto) => requestDto.ToEntity();

        protected override Equipment MapUpdatedAsEntity(UpdateEquipmentRequestDto requestDto) => requestDto.ToEntity();

        protected override IEntity MapToEntity(UpdateEquipmentRequestDto requestDto, Equipment oldEntity) => requestDto.ApplyTo(oldEntity);
    }
}
