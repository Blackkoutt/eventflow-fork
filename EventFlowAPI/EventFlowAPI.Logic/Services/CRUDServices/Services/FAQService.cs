using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
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
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class FAQService(
        IUnitOfWork unitOfWork,
        IAuthService authService) :
        IFAQService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;
        private readonly IGenericRepository<FAQ> _repository = unitOfWork.GetRepository<FAQ>();


        public async Task<Result<IEnumerable<FAQResponseDto>>> GetAllAsync(FAQQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<FAQResponseDto>>.Success(response);
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
                if (entity == null)
                    return Error.NotFound;
            }

            return Error.None;
        }

        private async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        entity.Question.ToLower() == ((IQuestionableRequestDto)requestDto).Question.ToLower()
                      ))).Any();

            return Result<bool>.Success(result);
        }

        private FAQResponseDto MapAsDto(FAQ entity) => entity.ToResponse();

        private FAQ MapAsEntity(FAQRequestDto requestDto) => requestDto.ToEntity();

        private IEntity MapToEntity(UpdateFAQRequestDto requestDto, FAQ oldEntity) => requestDto.ApplyTo(oldEntity);

        public async Task<Result<FAQResponseDto>> GetOneAsync(int id)
        {
            if (id < 0)
                return Result<FAQResponseDto>.Failure(Error.RouteParamOutOfRange);

            var record = await _repository.GetOneAsync(id);
            if (record == null)
                return Result<FAQResponseDto>.Failure(Error.NotFound);

            var response = MapAsDto(record);

            return Result<FAQResponseDto>.Success(response);
        }

        public async Task<Result<FAQResponseDto>> AddAsync(FAQRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto);
            if (error != Error.None)
                return Result<FAQResponseDto>.Failure(error);

            var entity = MapAsEntity(requestDto!);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<FAQResponseDto>.Success(response);
        }

        public async Task<Result<FAQResponseDto>> UpdateAsync(int id, UpdateFAQRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto, id);
            if (error != Error.None)
                return Result<FAQResponseDto>.Failure(error);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<FAQResponseDto>.Failure(Error.NotFound);

            await UpdateEntity(oldEntity, requestDto!);

            return Result<FAQResponseDto>.Success();
        }

        public async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            await DeleteEntity(validationResult.Value.Entity);

            return Result<object>.Success();
        }

        private async Task UpdateEntity(FAQ oldEntity, UpdateFAQRequestDto requestDto)
        {
            var newEntity = (FAQ)MapToEntity(requestDto, oldEntity);
            _repository.Update(newEntity);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task DeleteEntity(FAQ entity)
        {
            _repository.Delete(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        private IEnumerable<FAQResponseDto> MapAsDto(IEnumerable<FAQ> records)
        {
            return records.Select(entity => MapAsDto(entity));
        }

        private async Task<Result<(FAQ Entity, UserResponseDto User)>> ValidateBeforeDelete(int id)
        {
            if (id < 0)
                return Result<(FAQ Entity, UserResponseDto User)>.Failure(Error.RouteParamOutOfRange);

            var entity = await _repository.GetOneAsync(id);
            if (entity == null)
                return Result<(FAQ Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<(FAQ Entity, UserResponseDto User)>.Failure(userResult.Error);

            var user = userResult.Value;
            if (!user.IsInRole(Roles.Admin))
                return Result<(FAQ Entity, UserResponseDto User)>.Failure(user.IsInRole(Roles.User) ? AuthError.UserDoesNotHavePremissionToResource : AuthError.UserDoesNotHaveSpecificRole);

            return Result<(FAQ Entity, UserResponseDto User)>.Success((entity, user));
        }
    }
}
