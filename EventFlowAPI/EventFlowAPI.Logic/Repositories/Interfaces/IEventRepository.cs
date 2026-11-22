using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    /// <summary>
    /// Events are returned with their hall copy, category, festivals and tickets (with ticket types).
    /// "Active" means not deleted and not finished yet.
    /// </summary>
    public interface IEventRepository : IEntityRepository<Event>
    {
        Task<IReadOnlyList<Event>> GetPageAsync(EventQuery query);
        Task<IReadOnlyList<Event>> GetByIdsAsync(IEnumerable<int> ids);
        /// <summary>Active events with the given ids, ordered by start date.</summary>
        Task<IReadOnlyList<Event>> GetActiveByIdsAsync(IEnumerable<int> ids);
        Task<IReadOnlyList<Event>> GetActiveInHallsAsync(IEnumerable<int> hallIds);
        Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null);
        /// <summary>True when an event that is not over yet takes place in a copy of the hall at the same time.</summary>
        Task<bool> HasCollisionAsync(int defaultHallId, DateTime startDate, DateTime endDate, int? excludedEventId = null);
        // TODO (block 6): statistics should be computed in the database instead of loading every event
        Task<IReadOnlyList<Event>> GetAllAsync();
    }
}
