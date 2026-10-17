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
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class SponsorService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IFileService fileService) :
        GenericService<
            Sponsor,
            SponsorRequestDto,
            UpdateSponsorRequestDto,
            SponsorResponseDto,
            SponsorQuery
        >(unitOfWork, authService),
        ISponsorService
    {
        private readonly IFileService _fileService = fileService;
        public sealed override async Task<Result<IEnumerable<SponsorResponseDto>>> GetAllAsync(SponsorQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.Where(s => !s.IsDeleted)
                                                              .ByName(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<SponsorResponseDto>>.Success(response);
        }

        public sealed override async Task<Result<SponsorResponseDto>> AddAsync(SponsorRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<SponsorResponseDto>.Failure(validationError);

            var entity = MapAsEntity(requestDto!);

            entity.PartnerGuid = Guid.NewGuid();
            var photoPostError = await _fileService.PostPhoto(entity, requestDto!.SponsorPhoto, $"{entity.Name}_{entity.PartnerGuid}", isUpdate: false);
            if (photoPostError != Error.None)
                return Result<SponsorResponseDto>.Failure(photoPostError);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<SponsorResponseDto>.Success(response);
        }

        public sealed override async Task<Result<SponsorResponseDto>> UpdateAsync(int id, UpdateSponsorRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<SponsorResponseDto>.Failure(validationError);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<SponsorResponseDto>.Failure(Error.NotFound);

            var newEntity = (Sponsor)MapToEntity(requestDto!, oldEntity);

            var photoPostError = await _fileService.PostPhoto(newEntity, requestDto!.SponosorPhoto, $"{newEntity.Name}_{newEntity.PartnerGuid}", isUpdate: true);
            if (photoPostError != Error.None)
                return Result<SponsorResponseDto>.Failure(photoPostError);

            _repository.Update(newEntity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(newEntity);

            return Result<SponsorResponseDto>.Success(response);
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

        public new IEnumerable<SponsorResponseDto> MapAsDto(IEnumerable<Sponsor> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.AsDto<SponsorResponseDto>();
                responseDto.PhotoEndpoint = $"/Sponsors/{responseDto.Id}/image";
                return responseDto;
            });
        }

        public new SponsorResponseDto MapAsDto(Sponsor entity)
        {
            var responseDto = entity.AsDto<SponsorResponseDto>();
            responseDto.PhotoEndpoint = $"/Sponsors/{responseDto.Id}/image";
            return responseDto;
        }
    }
}
