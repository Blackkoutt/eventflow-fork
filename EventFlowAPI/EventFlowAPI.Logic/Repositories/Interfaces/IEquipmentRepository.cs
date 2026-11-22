using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface IEquipmentRepository : IEntityRepository<Equipment>
    {
        Task<IReadOnlyList<Equipment>> GetPageAsync(EquipmentQuery query);
        Task<IReadOnlyList<Equipment>> GetByIdsAsync(IEnumerable<int> ids);
        Task<bool> NameExistsAsync(string name, int? excludedId = null);
    }
}
