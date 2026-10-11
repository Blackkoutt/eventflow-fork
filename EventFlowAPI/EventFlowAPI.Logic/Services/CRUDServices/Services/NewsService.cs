using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class NewsService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IFileService fileService) :
        GenericService<
            News,
            NewsRequestDto,
            UpdateNewsRequestDto,
            NewsResponseDto,
            NewsQuery
        >(unitOfWork, authService),
        INewsService
    {
        private readonly IFileService _fileService = fileService;

        public sealed override async Task<Result<IEnumerable<NewsResponseDto>>> GetAllAsync(NewsQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<NewsResponseDto>>.Success(response);
        }

        public sealed override async Task<Result<NewsResponseDto>> AddAsync(NewsRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<NewsResponseDto>.Failure(validationError);

            var entity = MapAsEntity(requestDto!);

            entity.NewsGuid = Guid.NewGuid();
            var photoPostError = await _fileService.PostPhoto(entity, requestDto!.NewsPhoto, $"{entity.Title}_{entity.NewsGuid}", isUpdate: false);
            if (photoPostError != Error.None)
                return Result<NewsResponseDto>.Failure(photoPostError);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<NewsResponseDto>.Success(response);
        }

        public sealed override async Task<Result<NewsResponseDto>> UpdateAsync(int id, UpdateNewsRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<NewsResponseDto>.Failure(validationError);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<NewsResponseDto>.Failure(Error.NotFound);

            var newEntity = (News)MapToEntity(requestDto!, oldEntity);

            var photoPostError = await _fileService.PostPhoto(newEntity, requestDto!.NewsPhoto, $"{newEntity.Title}_{newEntity.NewsGuid}", isUpdate: true);
            if (photoPostError != Error.None)
                return Result<NewsResponseDto>.Failure(photoPostError);

            _repository.Update(newEntity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(newEntity);

            return Result<NewsResponseDto>.Success(response);
        }

        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            await _fileService.DeletePhoto(entity);
            await DeleteEntity(entity, isSoftDelete: false);

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
                        entity.Title.ToLower() == ((ITitleableRequestDto)requestDto).Title.ToLower()
                      ))).Any();

            return Result<bool>.Success(result);
        }

        protected sealed override IEnumerable<NewsResponseDto> MapAsDto(IEnumerable<News> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.AsDto<NewsResponseDto>();
                responseDto.PhotoEndpoint = $"/News/{responseDto.Id}/image";
                return responseDto;
            });
        }

        protected sealed override NewsResponseDto MapAsDto(News entity)
        {
            var responseDto = entity.AsDto<NewsResponseDto>();
            responseDto.PhotoEndpoint = $"/News/{responseDto.Id}/image";
            return responseDto;
        }
    }
}
