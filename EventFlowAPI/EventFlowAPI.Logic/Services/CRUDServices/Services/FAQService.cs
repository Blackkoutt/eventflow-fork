using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class FAQService(
        IUnitOfWork unitOfWork,
        IAuthService authService) :
        GenericService<
            FAQ,
            FAQRequestDto,
            UpdateFAQRequestDto,
            FAQResponseDto,
            FAQQuery
        >(unitOfWork, authService),
        IFAQService
    {

        public sealed override async Task<Result<IEnumerable<FAQResponseDto>>> GetAllAsync(FAQQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<FAQResponseDto>>.Success(response);
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
                if (entity == null)
                    return Error.NotFound;
            }

            return Error.None;
        }

        protected sealed override async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        entity.Question.ToLower() == ((IQuestionableRequestDto)requestDto).Question.ToLower()
                      ))).Any();

            return Result<bool>.Success(result);
        }

        protected override FAQResponseDto MapResponse(FAQ entity) => entity.ToResponse();

        protected override FAQ MapAsEntity(FAQRequestDto requestDto) => requestDto.ToEntity();

        protected override FAQ MapUpdatedAsEntity(UpdateFAQRequestDto requestDto) => requestDto.ToEntity();

        protected override IEntity MapToEntity(UpdateFAQRequestDto requestDto, FAQ oldEntity) => requestDto.ApplyTo(oldEntity);
    }
}
