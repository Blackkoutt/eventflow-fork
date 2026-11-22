using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface IAdditionalServicesRepository : IEntityRepository<AdditionalServices>
    {
        Task<IReadOnlyList<AdditionalServices>> GetPageAsync(AdditionalServicesQuery query);
        /// <summary>All services, soft-deleted ones included (old rents still show them).</summary>
        Task<IReadOnlyList<AdditionalServices>> GetAllAsync();
        Task<IReadOnlyList<AdditionalServices>> GetActiveByIdsAsync(IEnumerable<int> ids);
        Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null);
        /// <summary>True when any hall rent uses this service, so it can only be soft-deleted.</summary>
        Task<bool> IsUsedAsync(int id);
    }
}
