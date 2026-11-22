using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface IEventPassTypeRepository : IEntityRepository<EventPassType>
    {
        Task<IReadOnlyList<EventPassType>> GetPageAsync(EventPassTypeQuery query);
        /// <summary>All pass types, soft-deleted ones included (old passes still show them).</summary>
        Task<IReadOnlyList<EventPassType>> GetAllAsync();
        Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null);
        /// <summary>True when any event pass has this type, so it can only be soft-deleted.</summary>
        Task<bool> IsUsedAsync(int id);
    }
}
