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
    public sealed class AdditionalServicesService(IUnitOfWork unitOfWork, IAuthService authService) :
        IAdditionalServicesService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;
        private readonly IGenericRepository<AdditionalServices> _repository = unitOfWork.GetRepository<AdditionalServices>();

        public async Task<Result<IEnumerable<AdditionalServicesResponseDto>>> GetAllAsync(AdditionalServicesQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.Where(s => !s.IsDeleted)
                                                              .ByName(query)
                                                              .ByPrice(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<AdditionalServicesResponseDto>>.Success(response);
        }

        public async Task<Result<AdditionalServicesResponseDto>> UpdateAsync(int id, UpdateAdditionalServicesRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto, id);
            if (error != Error.None)
                return Result<AdditionalServicesResponseDto>.Failure(error);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<AdditionalServicesResponseDto>.Failure(Error.NotFound);

            await UpdateEntity(oldEntity, requestDto!);

            return Result<AdditionalServicesResponseDto>.Success();
        }


        public async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            await DeleteEntity(entity, isSoftDelete: entity.Rents.Any());

            return Result<object>.Success();
        }

        private async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            if (id != null && id < 0)
                return Error.RouteParamOutOfRange;

            if (requestDto == null)
                return Error.NullParameter;

            if(id == null)
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
                if (entity == null || entity.IsDeleted)
                    return Error.NotFound;
            }

            return Error.None;
        }


        private async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            if (requestDto is not INameableRequestDto) return Result<bool>.Failure(Error.BadRequestType);

            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower() &&
                        !entity.IsDeleted
                      ))).Any();

            return Result<bool>.Success(result);
        }

        private AdditionalServicesResponseDto MapAsDto(AdditionalServices entity) => entity.ToResponse();

        private AdditionalServices MapAsEntity(AdditionalServicesRequestDto requestDto) => requestDto.ToEntity();

        private IEntity MapToEntity(UpdateAdditionalServicesRequestDto requestDto, AdditionalServices oldEntity) => requestDto.ApplyTo(oldEntity);

        public async Task<Result<AdditionalServicesResponseDto>> GetOneAsync(int id)
        {
            if (id < 0)
                return Result<AdditionalServicesResponseDto>.Failure(Error.RouteParamOutOfRange);

            var record = await _repository.GetOneAsync(id);
            if (record == null)
                return Result<AdditionalServicesResponseDto>.Failure(Error.NotFound);

            var response = MapAsDto(record);

            return Result<AdditionalServicesResponseDto>.Success(response);
        }

        public async Task<Result<AdditionalServicesResponseDto>> AddAsync(AdditionalServicesRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto);
            if (error != Error.None)
                return Result<AdditionalServicesResponseDto>.Failure(error);

            var entity = MapAsEntity(requestDto!);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<AdditionalServicesResponseDto>.Success(response);
        }

        private async Task UpdateEntity(AdditionalServices oldEntity, UpdateAdditionalServicesRequestDto requestDto)
        {
            var newEntity = (AdditionalServices)MapToEntity(requestDto, oldEntity);
            _repository.Update(newEntity);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task DeleteEntity(AdditionalServices entity, bool isSoftDelete)
        {
            if (isSoftDelete)
            {
                entity.IsDeleted = true;
                entity.DeleteDate = DateTime.Now;
                _repository.Update(entity);
            }
            else
            {
                _repository.Delete(entity);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<Result<(AdditionalServices Entity, UserResponseDto User)>> ValidateBeforeDelete(int id)
        {
            if (id < 0)
                return Result<(AdditionalServices Entity, UserResponseDto User)>.Failure(Error.RouteParamOutOfRange);

            var entity = await _repository.GetOneAsync(id);
            if (entity == null)
                return Result<(AdditionalServices Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<(AdditionalServices Entity, UserResponseDto User)>.Failure(userResult.Error);

            if (entity.IsDeleted)
                return Result<(AdditionalServices Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var user = userResult.Value;
            if (!user.IsInRole(Roles.Admin))
                return Result<(AdditionalServices Entity, UserResponseDto User)>.Failure(user.IsInRole(Roles.User) ? AuthError.UserDoesNotHavePremissionToResource : AuthError.UserDoesNotHaveSpecificRole);

            return Result<(AdditionalServices Entity, UserResponseDto User)>.Success((entity, user));
        }

        private IEnumerable<AdditionalServicesResponseDto> MapAsDto(IEnumerable<AdditionalServices> records)
        {
            return records.Select(entity => MapAsDto(entity));
        }
    }
}
