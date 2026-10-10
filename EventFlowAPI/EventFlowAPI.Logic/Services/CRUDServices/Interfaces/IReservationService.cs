using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IReservationService :
        IGenericService<
            Reservation,
            ReservationRequestDto,
            UpdateReservationRequestDto,
            ReservationResponseDto,
            ReservationQuery
        >
    {
        Task<IEnumerable<Reservation>> GetActiveReservationsForFestival(int festivalId);
        Task<Result<IEnumerable<ReservationResponseDto>>> MakeReservation(ReservationRequestDto? requestDto);
        Task<Error> UpdateReservationsTickets(IEnumerable<Reservation> reservations);
        Task<IEnumerable<Reservation>> GetActiveReservationsForEvent(int eventId);
        Task<Error> SoftDeleteReservationAndFileTickets(Reservation reservation, bool deleteForFestival = false);
        Task<Error> CancelReservationsInCauseOfDeleteEventOrHallOrFestival(IEnumerable<Reservation> reservations);
    }
}
