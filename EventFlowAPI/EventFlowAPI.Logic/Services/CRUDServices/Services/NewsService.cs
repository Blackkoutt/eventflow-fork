using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
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
using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class NewsService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IFileService fileService) :
        INewsService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;
        private readonly IGenericRepository<News> _repository = unitOfWork.GetRepository<News>();

        private readonly IFileService _fileService = fileService;

        public async Task<Result<IEnumerable<NewsResponseDto>>> GetAllAsync(NewsQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<NewsResponseDto>>.Success(response);
        }

        public async Task<Result<NewsResponseDto>> AddAsync(NewsRequestDto? requestDto)
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

        public async Task<Result<NewsResponseDto>> UpdateAsync(int id, UpdateNewsRequestDto? requestDto)
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

        public async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            await _fileService.DeletePhoto(entity);
            await DeleteEntity(entity);

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
                        entity.Title.ToLower() == ((ITitleableRequestDto)requestDto).Title.ToLower()
                      ))).Any();

            return Result<bool>.Success(result);
        }

        private IEnumerable<NewsResponseDto> MapAsDto(IEnumerable<News> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.ToResponse();
                responseDto.PhotoEndpoint = $"/News/{responseDto.Id}/image";
                return responseDto;
            });
        }

        private NewsResponseDto MapAsDto(News entity)
        {
            var responseDto = entity.ToResponse();
            responseDto.PhotoEndpoint = $"/News/{responseDto.Id}/image";
            return responseDto;
        }

        private News MapAsEntity(NewsRequestDto requestDto) => requestDto.ToEntity();

        private IEntity MapToEntity(UpdateNewsRequestDto requestDto, News oldEntity) => requestDto.ApplyTo(oldEntity);

        public async Task<Result<NewsResponseDto>> GetOneAsync(int id)
        {
            if (id < 0)
                return Result<NewsResponseDto>.Failure(Error.RouteParamOutOfRange);

            var record = await _repository.GetOneAsync(id);
            if (record == null)
                return Result<NewsResponseDto>.Failure(Error.NotFound);

            var response = MapAsDto(record);

            return Result<NewsResponseDto>.Success(response);
        }

        private async Task DeleteEntity(News entity)
        {
            _repository.Delete(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<Result<(News Entity, UserResponseDto User)>> ValidateBeforeDelete(int id)
        {
            if (id < 0)
                return Result<(News Entity, UserResponseDto User)>.Failure(Error.RouteParamOutOfRange);

            var entity = await _repository.GetOneAsync(id);
            if (entity == null)
                return Result<(News Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<(News Entity, UserResponseDto User)>.Failure(userResult.Error);

            var user = userResult.Value;
            if (!user.IsInRole(Roles.Admin))
                return Result<(News Entity, UserResponseDto User)>.Failure(user.IsInRole(Roles.User) ? AuthError.UserDoesNotHavePremissionToResource : AuthError.UserDoesNotHaveSpecificRole);

            return Result<(News Entity, UserResponseDto User)>.Success((entity, user));
        }
    }
}
