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
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class PaymentTypeService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IFileService fileService) :
        GenericService<
            PaymentType,
            PaymentTypeRequestDto,
            UpdatePaymentTypeRequestDto,
            PaymentTypeResponseDto,
            PaymentTypeQuery
        >(unitOfWork, authService),
        IPaymentTypeService
    {
        private readonly IFileService _fileService = fileService;
        public sealed override async Task<Result<IEnumerable<PaymentTypeResponseDto>>> GetAllAsync(PaymentTypeQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.Where(pt => !pt.IsDeleted)
                                                              .ByName(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<PaymentTypeResponseDto>>.Success(response);
        }

        public sealed override async Task<Result<PaymentTypeResponseDto>> AddAsync(PaymentTypeRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<PaymentTypeResponseDto>.Failure(validationError);

            var entity = MapAsEntity(requestDto!);

            entity.PaymentTypeGuid = Guid.NewGuid();
            var photoPostError = await _fileService.PostPhoto(entity, requestDto!.PaymentTypePhoto, $"{entity.Name}_{entity.PaymentTypeGuid}", isUpdate: false);
            if (photoPostError != Error.None)
                return Result<PaymentTypeResponseDto>.Failure(photoPostError);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<PaymentTypeResponseDto>.Success(response);
        }

        public sealed override async Task<Result<PaymentTypeResponseDto>> UpdateAsync(int id, UpdatePaymentTypeRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<PaymentTypeResponseDto>.Failure(validationError);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<PaymentTypeResponseDto>.Failure(Error.NotFound);

            var newEntity = (PaymentType)MapToEntity(requestDto!, oldEntity);

            var photoPostError = await _fileService.PostPhoto(newEntity, requestDto!.PaymentTypePhoto, $"{newEntity.Name}_{newEntity.PaymentTypeGuid}", isUpdate: true);
            if (photoPostError != Error.None)
                return Result<PaymentTypeResponseDto>.Failure(photoPostError);

            _repository.Update(newEntity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(newEntity);

            return Result<PaymentTypeResponseDto>.Success(response);
        }

        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            bool isSoftDelete = entity.HallRents.Any() || entity.EventPasses.Any() || entity.Reservations.Any();

            await _fileService.DeletePhoto(entity);
            await DeleteEntity(entity, isSoftDelete: isSoftDelete);

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

        protected sealed override IEnumerable<PaymentTypeResponseDto> MapAsDto(IEnumerable<PaymentType> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.ToResponse();
                responseDto.PhotoEndpoint = $"/PaymentTypes/{responseDto.Id}/image";
                return responseDto;
            });
        }

        protected sealed override PaymentTypeResponseDto MapAsDto(PaymentType entity)
        {
            var responseDto = entity.ToResponse();
            responseDto.PhotoEndpoint = $"/PaymentTypes/{responseDto.Id}/image";
            return responseDto;
        }

        protected override PaymentTypeResponseDto MapResponse(PaymentType entity) => entity.ToResponse();

        protected override PaymentType MapAsEntity(PaymentTypeRequestDto requestDto) => requestDto.ToEntity();

        protected override PaymentType MapUpdatedAsEntity(UpdatePaymentTypeRequestDto requestDto) => requestDto.ToEntity();

        protected override IEntity MapToEntity(UpdatePaymentTypeRequestDto requestDto, PaymentType oldEntity) => requestDto.ApplyTo(oldEntity);
    }
}
