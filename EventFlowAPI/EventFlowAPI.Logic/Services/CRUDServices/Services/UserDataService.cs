using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class UserDataService(IUnitOfWork unitOfWork, IAuthService authService) :
        GenericService<
            UserData,
            UserDataRequestDto,
            UpdateUserDataRequestDto,
            UserDataResponseDto,
            UserDataQuery
        >(unitOfWork, authService),
        IUserDataService
    {
        public override Task<Result<IEnumerable<UserDataResponseDto>>> GetAllAsync(UserDataQuery query)
        {
            throw new NotImplementedException();
        }

        protected override Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            throw new NotImplementedException();
        }

        protected override Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            throw new NotImplementedException();
        }

        protected override UserDataResponseDto MapResponse(UserData entity) => entity.ToResponse();

        protected override UserData MapAsEntity(UserDataRequestDto requestDto) => requestDto.ToEntity();

        protected override UserData MapUpdatedAsEntity(UpdateUserDataRequestDto requestDto) => requestDto.ToEntity();

        protected override IEntity MapToEntity(UpdateUserDataRequestDto requestDto, UserData oldEntity) => requestDto.ApplyTo(oldEntity);
    }
}
