using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
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
        IEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;
        private readonly IGenericRepository<Equipment> _repository = unitOfWork.GetRepository<Equipment>();

        public async Task<Result<IEnumerable<EquipmentResponseDto>>> GetAllAsync(EquipmentQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.ByName(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<EquipmentResponseDto>>.Success(response);
        }

        private async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
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


        private async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            if (requestDto is not INameableRequestDto) return Result<bool>.Failure(Error.BadRequestType);

            var result = (await _repository.GetAllAsync(q =>
                          q.Where(entity =>
                            entity.Id != id &&
                            entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower()))).Any();

            return Result<bool>.Success(result);
        }

        private EquipmentResponseDto MapAsDto(Equipment entity) => entity.ToResponse();

        private Equipment MapAsEntity(EquipmentRequestDto requestDto) => requestDto.ToEntity();

        private IEntity MapToEntity(UpdateEquipmentRequestDto requestDto, Equipment oldEntity) => requestDto.ApplyTo(oldEntity);

        public async Task<Result<EquipmentResponseDto>> GetOneAsync(int id)
        {
            if (id < 0)
                return Result<EquipmentResponseDto>.Failure(Error.RouteParamOutOfRange);

            var record = await _repository.GetOneAsync(id);
            if (record == null)
                return Result<EquipmentResponseDto>.Failure(Error.NotFound);

            var response = MapAsDto(record);

            return Result<EquipmentResponseDto>.Success(response);
        }

        public async Task<Result<EquipmentResponseDto>> AddAsync(EquipmentRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto);
            if (error != Error.None)
                return Result<EquipmentResponseDto>.Failure(error);

            var entity = MapAsEntity(requestDto!);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<EquipmentResponseDto>.Success(response);
        }

        public async Task<Result<EquipmentResponseDto>> UpdateAsync(int id, UpdateEquipmentRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto, id);
            if (error != Error.None)
                return Result<EquipmentResponseDto>.Failure(error);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<EquipmentResponseDto>.Failure(Error.NotFound);

            await UpdateEntity(oldEntity, requestDto!);

            return Result<EquipmentResponseDto>.Success();
        }

        public async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            await DeleteEntity(validationResult.Value.Entity);

            return Result<object>.Success();
        }

        private async Task UpdateEntity(Equipment oldEntity, UpdateEquipmentRequestDto requestDto)
        {
            var newEntity = (Equipment)MapToEntity(requestDto, oldEntity);
            _repository.Update(newEntity);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task DeleteEntity(Equipment entity)
        {
            _repository.Delete(entity);

            await _unitOfWork.SaveChangesAsync();
        }

        private IEnumerable<EquipmentResponseDto> MapAsDto(IEnumerable<Equipment> records)
        {
            return records.Select(entity => MapAsDto(entity));
        }

        private async Task<Result<(Equipment Entity, UserResponseDto User)>> ValidateBeforeDelete(int id)
        {
            if (id < 0)
                return Result<(Equipment Entity, UserResponseDto User)>.Failure(Error.RouteParamOutOfRange);

            var entity = await _repository.GetOneAsync(id);
            if (entity == null)
                return Result<(Equipment Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<(Equipment Entity, UserResponseDto User)>.Failure(userResult.Error);

            var user = userResult.Value;
            if (!user.IsInRole(Roles.Admin))
                return Result<(Equipment Entity, UserResponseDto User)>.Failure(user.IsInRole(Roles.User) ? AuthError.UserDoesNotHavePremissionToResource : AuthError.UserDoesNotHaveSpecificRole);

            return Result<(Equipment Entity, UserResponseDto User)>.Success((entity, user));
        }
    }
}
