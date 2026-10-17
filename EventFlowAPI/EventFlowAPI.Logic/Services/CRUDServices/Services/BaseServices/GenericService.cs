using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Query.Abstract;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces.BaseInterfaces;
using EventFlowAPI.Logic.UnitOfWork;
using Serilog;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices
{
    public abstract class GenericService<
        TEntity,
        TRequestDto,
        TUpdateRequestDto,
        TResponseDto,
        TQuery>
        (IUnitOfWork unitOfWork, IAuthService authService) : IGenericService<
        TEntity,
        TRequestDto,
        TUpdateRequestDto,
        TResponseDto,
        TQuery>
        where TEntity : class, IEntity
        where TRequestDto : class, IRequestDto
        where TUpdateRequestDto : class, IRequestDto
        where TResponseDto : class, IResponseDto
        where TQuery : QueryObject
    {
        //AutoMapperMappingException , RepositoryNotExist
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected readonly IAuthService _authService = authService;
        protected readonly IGenericRepository<TEntity> _repository = unitOfWork.GetRepository<TEntity>();


        public abstract Task<Result<IEnumerable<TResponseDto>>> GetAllAsync(TQuery query);

        public virtual async Task<Result<TResponseDto>> GetOneAsync(int id)
        {
            if (id < 0)
                return Result<TResponseDto>.Failure(Error.RouteParamOutOfRange);

            var record = await _repository.GetOneAsync(id);
            if (record == null)
                return Result<TResponseDto>.Failure(Error.NotFound);

            var response = MapAsDto(record);

            return Result<TResponseDto>.Success(response);
        }


        public virtual async Task<Result<TResponseDto>> AddAsync(TRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto);
            if (error != Error.None)
                return Result<TResponseDto>.Failure(error);

            var entity = MapAsEntity(requestDto!);

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(entity);

            return Result<TResponseDto>.Success(response);
        }


        public virtual async Task<Result<TResponseDto>> UpdateAsync(int id, TUpdateRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto, id);
            if (error != Error.None)
                return Result<TResponseDto>.Failure(error);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<TResponseDto>.Failure(Error.NotFound);

            await UpdateEntity(oldEntity, requestDto!);

            return Result<TResponseDto>.Success();
        }

        protected async Task UpdateEntity(TEntity oldEntity, TUpdateRequestDto requestDto)
        {
            var newEntity = (TEntity)MapToEntity(requestDto!, oldEntity);
            _repository.Update(newEntity);

            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            await DeleteEntity(entity, isSoftDelete: false);

            return Result<object>.Success();
        }

        protected async Task DeleteEntity(TEntity entity, bool isSoftDelete)
        {
            if (entity is ISoftDeleteable softDeleteableEntity && isSoftDelete)
            {
                softDeleteableEntity.IsDeleted = true;
                softDeleteableEntity.DeleteDate = DateTime.Now;
                _repository.Update(entity);
            }
            else
            {
                _repository.Delete(entity);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        protected virtual async Task<Result<(TEntity Entity, UserResponseDto User)>> ValidateBeforeDelete(int id)
        {
            if (id < 0)
                return Result<(TEntity Entity, UserResponseDto User)>.Failure(Error.RouteParamOutOfRange);

            var entity = await _repository.GetOneAsync(id);

            if (entity == null)
                return Result<(TEntity Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<(TEntity Entity, UserResponseDto User)>.Failure(userResult.Error);

            if (entity is ISoftDeleteable && ((ISoftDeleteable)entity).IsDeleted)
                return Result<(TEntity Entity, UserResponseDto User)>.Failure(Error.NotFound);

            if (entity is IExpireable && ((IExpireable)entity).IsExpired)
                return Result<(TEntity Entity, UserResponseDto User)>.Failure(Error.EntityIsExpired);

            if(entity is IVisibleEntity && !((IVisibleEntity)entity).IsVisible)
                return Result<(TEntity Entity, UserResponseDto User)>.Failure(Error.NotFound);

            var user = userResult.Value;
            string? userId = null;
            if(entity is IAuthEntity authEntity)
                userId = authEntity.UserId;

            var premissionError = CheckUserPremission(user, userId);
            if(premissionError != Error.None)
                return Result<(TEntity Entity, UserResponseDto User)>.Failure(premissionError);

            return Result<(TEntity Entity, UserResponseDto User)>.Success((entity, user));
        }


        protected Error CheckUserPremission(UserResponseDto user, string? userId = null)
        {
            if (user.IsInRole(Roles.Admin)) 
                return Error.None;
            else if (user.IsInRole(Roles.User))
            {
                if (userId == user.Id)
                    return Error.None;
                else
                    return AuthError.UserDoesNotHavePremissionToResource;
            }                
            else
                return AuthError.UserDoesNotHaveSpecificRole;
        }

        protected Status GetEntityStatus(TEntity entity)
        {
            if(entity is ISoftDeleteable deleteableEntity && entity is IDateableEntity dateableEntity)
            {
                if (!deleteableEntity.IsDeleted && dateableEntity.EndDate > DateTime.Now) return Status.Active;
                else if (!deleteableEntity.IsDeleted && dateableEntity.EndDate < DateTime.Now) return Status.Expired;
                else if (deleteableEntity.IsDeleted) return Status.Canceled;
            }
            return Status.Unknown;
        }

        protected virtual IEnumerable<TResponseDto> MapAsDto(IEnumerable<TEntity> records) => records.Select(entity => entity.AsDto<TResponseDto>());
        protected virtual TResponseDto MapAsDto(TEntity entity) => entity.AsDto<TResponseDto>();
        protected virtual TEntity MapAsEntity(IRequestDto requestDto) => requestDto.AsEntity<TEntity>();
        protected virtual IEntity MapToEntity(TUpdateRequestDto requestDto, TEntity oldEntity) => requestDto.MapTo(oldEntity);
        protected async Task<bool> IsEntityExistInDB<TSomeEntity>(int id)
                    where TSomeEntity : class =>
                    await _unitOfWork.GetRepository<TSomeEntity>().GetOneAsync(id) != null;

        protected abstract Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null);
        protected abstract Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null);
    }
}
