using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    /// <summary>Hall types are returned together with their equipment.</summary>
    public interface IHallTypeRepository : IEntityRepository<HallType>
    {
        Task<IReadOnlyList<HallType>> GetPageAsync(HallTypeQuery query);
        Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null);
        /// <summary>True when any hall has this type, so it can only be soft-deleted.</summary>
        Task<bool> IsUsedAsync(int id);
    }
}
