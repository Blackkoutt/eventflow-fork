using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
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
    public sealed class PartnerService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IFileService fileService) :
        GenericService<
            Partner,
            PartnerRequestDto,
            UpdatePartnerRequestDto,
            PartnerResponseDto,
            PartnerQuery
        >(unitOfWork, authService),
        IPartnerService
    {
        private readonly IFileService _fileService = fileService;

        public sealed override async Task<Result<IEnumerable<PartnerResponseDto>>> GetAllAsync(PartnerQuery query)
        {
            var records = await _repository.GetAllAsync(q =>
            {
                var partners = q.Where(p => !p.IsDeleted);
                if (query.Kind != null)
                {
                    partners = partners.Where(p => p.Kind == query.Kind);
                }
                return partners.ByName(query)
                               .SortBy(query.SortBy, query.SortDirection)
                               .GetPage(query.PageNumber, query.PageSize);
            });
            var response = MapAsDto(records);
            return Result<IEnumerable<PartnerResponseDto>>.Success(response);
        }

        public sealed override async Task<Result<PartnerResponseDto>> AddAsync(PartnerRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<PartnerResponseDto>.Failure(validationError);

            var entity = MapAsEntity(requestDto!);
            entity.Kind = requestDto!.Kind!.Value;

            entity.PartnerGuid = Guid.NewGuid();
            var photoPostError = await _fileService.PostPhoto(entity, requestDto.PartnerPhoto, $"{entity.Name}_{entity.PartnerGuid}", isUpdate: false);
            if (photoPostError != Error.None)
                return Result<PartnerResponseDto>.Failure(photoPostError);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<PartnerResponseDto>.Success(response);
        }

        public sealed override async Task<Result<PartnerResponseDto>> UpdateAsync(int id, UpdatePartnerRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<PartnerResponseDto>.Failure(validationError);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<PartnerResponseDto>.Failure(Error.NotFound);

            var newEntity = (Partner)MapToEntity(requestDto!, oldEntity);

            var photoPostError = await _fileService.PostPhoto(newEntity, requestDto!.PartnerPhoto, $"{newEntity.Name}_{newEntity.PartnerGuid}", isUpdate: true);
            if (photoPostError != Error.None)
                return Result<PartnerResponseDto>.Failure(photoPostError);

            _repository.Update(newEntity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(newEntity);

            return Result<PartnerResponseDto>.Success(response);
        }

        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;

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
            PartnerKind? kind = requestDto is PartnerRequestDto addRequestDto ? addRequestDto.Kind : null;
            if (kind == null && id != null)
            {
                var existingEntity = await _repository.GetOneAsync((int)id);
                kind = existingEntity?.Kind;
            }

            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        entity.Kind == kind &&
                        entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower() &&
                        !entity.IsDeleted
                      ))).Any();

            return Result<bool>.Success(result);
        }

        public new IEnumerable<PartnerResponseDto> MapAsDto(IEnumerable<Partner> records)
        {
            return records.Select(entity => MapAsDto(entity));
        }

        public new PartnerResponseDto MapAsDto(Partner entity)
        {
            var responseDto = entity.AsDto<PartnerResponseDto>();
            responseDto.PhotoEndpoint = $"/Partners/{responseDto.Id}/image";
            return responseDto;
        }
    }
}
