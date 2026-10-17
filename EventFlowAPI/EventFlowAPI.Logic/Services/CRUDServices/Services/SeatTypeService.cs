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
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class SeatTypeService(IUnitOfWork unitOfWork, IAuthService authService) :
        GenericService<
            SeatType,
            SeatTypeRequestDto,
            UpdateSeatTypeRequestDto,
            SeatTypeResponseDto,
            SeatTypeQuery
        >(unitOfWork, authService),
        ISeatTypeService
    {
        public sealed override async Task<Result<IEnumerable<SeatTypeResponseDto>>> GetAllAsync(SeatTypeQuery query)
        {
            var records = await _repository.GetAllAsync(q => q.Where(st => !st.IsDeleted)
                                                              .ByName(query)
                                                              .SortBy(query.SortBy, query.SortDirection)
                                                              .GetPage(query.PageNumber, query.PageSize));
            var response = MapAsDto(records);
            return Result<IEnumerable<SeatTypeResponseDto>>.Success(response);
        }

        public sealed override async Task<Result<SeatTypeResponseDto>> UpdateAsync(int id, UpdateSeatTypeRequestDto? requestDto)
        {
            var error = await ValidateEntity(requestDto, id);
            if (error != Error.None)
                return Result<SeatTypeResponseDto>.Failure(error);

            var oldEntity = await _repository.GetOneAsync(id);
            if (oldEntity == null)
                return Result<SeatTypeResponseDto>.Failure(Error.NotFound);

            var newEntity = oldEntity;

            newEntity = (SeatType)MapToEntity(requestDto!, newEntity);
            _repository.Update(newEntity);

            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(newEntity);

            return Result<SeatTypeResponseDto>.Success();
        }

        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var entity = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            if(entity.Name == "Miejsce zwykłe")
                return Result<object>.Failure(SeatTypeError.CannotDeleteDefaultSeat);

            var defaultSeatType = (await _repository.GetAllAsync(q => q.Where(s => s.Name == "Miejsce zwykłe"))).FirstOrDefault();
            if (defaultSeatType == null) return Result<object>.Failure(SeatTypeError.CannotFoundDefaultSeat);
            var seatsWithoutReservations = entity.Seats.Where(s => !s.Reservations.Any());
            foreach (var seat in seatsWithoutReservations)
            {
                seat.SeatType = defaultSeatType;
                _unitOfWork.GetRepository<Seat>().Update(seat);
            }

            if (entity is ISoftDeleteable softDeleteableEntity && entity.Seats.Any(s => s.Reservations.Any()))
            {
                softDeleteableEntity.IsDeleted = true;
                softDeleteableEntity.DeleteDate = DateTime.Now;
                entity.Seats = entity.Seats.Where(s => s.Reservations.Any()).ToList();
                _repository.Update(entity);
            }
            else
            {
                _repository.Delete(entity);
            }          

            await _unitOfWork.SaveChangesAsync();

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

                if (entity.Name == "Miejsce zwykłe")
                    return SeatTypeError.CannotDeleteDefaultSeat;
            }

            return Error.None;
        }

        protected sealed override async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            var result = (await _repository.GetAllAsync(q =>
                      q.Where(entity =>
                        entity.Id != id &&
                        (entity.Name.ToLower() == ((INameableRequestDto)requestDto).Name.ToLower() ||
                         entity.SeatColor == ((IColorableRequestDto)requestDto).SeatColor) &&
                        !entity.IsDeleted
                      ))).Any();

            return Result<bool>.Success(result);
        }
    }
}
