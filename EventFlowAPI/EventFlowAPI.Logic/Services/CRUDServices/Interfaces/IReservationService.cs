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
        Task<Result<PaymentRedirectDto>> CreateReservationPayment(ReservationRequestDto? requestDto);
        Task<Result<IEnumerable<ReservationResponseDto>>> MakeReservation(ReservationRequestDto? requestDto);
        Task<Error> SendMailsAboutUpdatedReservations<TEntity>(IEnumerable<Reservation> reservationsForEvent, TEntity? oldEntity = null, TEntity? newEntity = null) where TEntity : class;
        Task<IEnumerable<Reservation>> GetActiveReservationsForEvent(int eventId);
        Task<Error> SoftDeleteReservationAndFileTickets(Reservation reservation, bool deleteForFestival = false);
        Task<Error> CancelReservationsInCauseOfDeleteEventOrHallOrFestival(IEnumerable<Reservation> reservations, Event? eventEntity = null, Festival? festival = null);
    }
}
