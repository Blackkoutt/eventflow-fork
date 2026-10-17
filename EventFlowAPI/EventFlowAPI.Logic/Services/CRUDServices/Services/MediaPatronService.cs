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
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class MediaPatronService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IFileService fileService) :
        GenericService<
            MediaPatron,
            MediaPatronRequestDto,
            UpdateMediaPatronRequestDto,
            MediaPatronResponseDto,
            MediaPatronQuery
        >(unitOfWork, authService),
        IMediaPatronService
    {
        private readonly IFileService _fileService = fileService;
        public sealed override async Task<Result<IEnumerable<MediaPatronResponseDto>>> GetAllAsync(MediaPatronQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.Where(s => !s.IsDeleted)
                                                              .ByName(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<MediaPatronResponseDto>>.Success(response);
        }

        public sealed override async Task<Result<MediaPatronResponseDto>> AddAsync(MediaPatronRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<MediaPatronResponseDto>.Failure(validationError);

            var entity = MapAsEntity(requestDto!);

            entity.PartnerGuid = Guid.NewGuid();
            var photoPostError = await _fileService.PostPhoto(entity, requestDto!.MediaPatronPhoto, $"{entity.Name}_{entity.PartnerGuid}", isUpdate: false);
            if (photoPostError != Error.None)
                return Result<MediaPatronResponseDto>.Failure(photoPostError);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<MediaPatronResponseDto>.Success(response);
        }

        public sealed override async Task<Result<MediaPatronResponseDto>> UpdateAsync(int id, UpdateMediaPatronRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<MediaPatronResponseDto>.Failure(validationError);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<MediaPatronResponseDto>.Failure(Error.NotFound);

            var newEntity = (MediaPatron)MapToEntity(requestDto!, oldEntity);

            var photoPostError = await _fileService.PostPhoto(newEntity, requestDto!.MediaPatronPhoto, $"{newEntity.Name}_{newEntity.PartnerGuid}", isUpdate: true);
            if (photoPostError != Error.None)
                return Result<MediaPatronResponseDto>.Failure(photoPostError);

            _repository.Update(newEntity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(newEntity);

            return Result<MediaPatronResponseDto>.Success(response);
        }

        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            await _fileService.DeletePhoto(entity);
            await DeleteEntity(entity, isSoftDelete: entity.Festivals.Any());

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
                if (entity == null || entity.IsDeleted)
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
                        !entity.IsDeleted
                      ))).Any();

            return Result<bool>.Success(result);
        }

        public new IEnumerable<MediaPatronResponseDto> MapAsDto(IEnumerable<MediaPatron> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.AsDto<MediaPatronResponseDto>();
                responseDto.PhotoEndpoint = $"/MediaPatrons/{responseDto.Id}/image";
                return responseDto;
            });
        }

        public new MediaPatronResponseDto MapAsDto(MediaPatron entity)
        {
            var responseDto = entity.AsDto<MediaPatronResponseDto>();
            responseDto.PhotoEndpoint = $"/MediaPatrons/{responseDto.Id}/image";
            return responseDto;
        }
    }
}
