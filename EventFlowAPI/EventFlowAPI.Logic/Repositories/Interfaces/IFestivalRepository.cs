using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    /// <summary>
    /// Festivals are returned with their events (with category and hall), partners and tickets (with ticket types).
    /// The events and partners of a festival are changed through the festival.
    /// </summary>
    public interface IFestivalRepository : IEntityRepository<Festival>
    {
        Task<IReadOnlyList<Festival>> GetPageAsync(FestivalQuery query);
        /// <summary>Festivals that are not deleted or finished and contain any of the events.</summary>
        Task<IReadOnlyList<Festival>> GetActiveWithAnyEventAsync(IEnumerable<int> eventIds);
        Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null);
        // TODO (block 6): statistics should be computed in the database instead of loading every festival
        Task<IReadOnlyList<Festival>> GetAllAsync();
    }
}
