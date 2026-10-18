using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IReservationService
    {
        Task<Result<ReservationResponseDto>> AddAsync(ReservationRequestDto? requestDto);
        Task<Result<IEnumerable<ReservationResponseDto>>> GetAllAsync(ReservationQuery query);
        Task<Result<ReservationResponseDto>> GetOneAsync(int id);
        Task<Result<ReservationResponseDto>> UpdateAsync(int id, UpdateReservationRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
        Task<IEnumerable<Reservation>> GetActiveReservationsForFestival(int festivalId);
        Task<Result<IEnumerable<ReservationResponseDto>>> MakeReservation(ReservationRequestDto? requestDto);
        Task<IEnumerable<Reservation>> GetActiveReservationsForEvent(int eventId);
        Task<Error> CancelReservationsInCauseOfDeleteEventOrHallOrFestival(IEnumerable<Reservation> reservations);
    }
}
