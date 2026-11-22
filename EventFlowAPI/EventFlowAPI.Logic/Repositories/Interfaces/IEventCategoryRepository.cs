using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface IEventCategoryRepository : IEntityRepository<EventCategory>
    {
        Task<IReadOnlyList<EventCategory>> GetPageAsync(EventCategoryQuery query);
        Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null);
        /// <summary>True when any event has this category, so it can only be soft-deleted.</summary>
        Task<bool> IsUsedAsync(int id);
    }
}
