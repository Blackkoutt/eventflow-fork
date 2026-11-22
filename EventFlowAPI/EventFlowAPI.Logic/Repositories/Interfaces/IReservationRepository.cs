using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    /// <summary>
    /// Reservations are returned with their user, seats (with seat types) and ticket
    /// (with ticket type, event with hall and category, and festival).
    /// "Active" means not cancelled and not finished yet.
    /// </summary>
    public interface IReservationRepository : IEntityRepository<Reservation>
    {
        /// <summary>One page of reservations; only the user's own when userId is given.</summary>
        Task<IReadOnlyList<Reservation>> GetPageAsync(ReservationQuery query, string? userId = null);
        /// <summary>All reservations made in one purchase (a festival purchase has one per event).</summary>
        Task<IReadOnlyList<Reservation>> GetByGuidAsync(Guid reservationGuid);
        /// <summary>The user's other reservations for the same festival.</summary>
        Task<IReadOnlyList<Reservation>> GetOtherFestivalReservationsAsync(int excludedReservationId, string userId, int festivalId);
        Task<IReadOnlyList<Reservation>> GetActiveForEventAsync(int eventId);
        Task<IReadOnlyList<Reservation>> GetActiveForFestivalAsync(int festivalId);
        /// <summary>Not cancelled reservations for the festival whose time has already passed.</summary>
        Task<IReadOnlyList<Reservation>> GetFinishedForFestivalAsync(int festivalId);
        /// <summary>Active reservations for events held in the given hall copies.</summary>
        Task<IReadOnlyList<Reservation>> GetActiveInHallsAsync(IEnumerable<int> hallIds);
        /// <summary>Active reservations for events held in any copy of the original hall.</summary>
        Task<IReadOnlyList<Reservation>> GetActiveForDefaultHallAsync(int? defaultHallId);
        Task<IReadOnlyList<Reservation>> GetActiveForEventPassAsync(int eventPassId);
    }
}
