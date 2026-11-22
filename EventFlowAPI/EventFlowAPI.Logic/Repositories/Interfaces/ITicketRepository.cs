using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    /// <summary>Tickets are returned with their type, event (with category and hall) and festival (with events).</summary>
    public interface ITicketRepository : IEntityRepository<Ticket>
    {
        /// <summary>Tickets sold for the whole festival (one per event and ticket type).</summary>
        Task<IReadOnlyList<Ticket>> GetByFestivalAsync(int festivalId);
        /// <summary>Tickets sold for a single event, without festival tickets.</summary>
        Task<IReadOnlyList<Ticket>> GetEventTicketsAsync(int eventId);
        /// <summary>Not deleted tickets of the events, and all tickets of the festivals.</summary>
        Task<IReadOnlyList<Ticket>> GetForEventsOrFestivalsAsync(IEnumerable<int> eventIds, IEnumerable<int> festivalIds);
    }
}
