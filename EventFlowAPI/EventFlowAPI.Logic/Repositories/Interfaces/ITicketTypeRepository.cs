using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface ITicketTypeRepository : IEntityRepository<TicketType>
    {
        Task<IReadOnlyList<TicketType>> GetPageAsync(TicketTypeQuery query);
        Task<IReadOnlyList<TicketType>> GetActiveAsync();
        Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null);
        /// <summary>True when any ticket uses this type, so it can only be soft-deleted.</summary>
        Task<bool> IsUsedAsync(int id);
    }
}
