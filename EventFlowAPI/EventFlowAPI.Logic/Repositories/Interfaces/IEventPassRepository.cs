using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    /// <summary>Event passes are returned with their user and pass type.</summary>
    public interface IEventPassRepository : IEntityRepository<EventPass>
    {
        /// <summary>One page of passes; only the user's own when userId is given.</summary>
        Task<IReadOnlyList<EventPass>> GetPageAsync(EventPassQuery query, string? userId = null);
        /// <summary>The user's pass that is not cancelled and still valid, if any.</summary>
        Task<EventPass?> GetActiveForUserAsync(string userId);
    }
}
