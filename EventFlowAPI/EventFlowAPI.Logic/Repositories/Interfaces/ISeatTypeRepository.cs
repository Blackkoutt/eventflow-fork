using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    public interface ISeatTypeRepository : IEntityRepository<SeatType>
    {
        Task<IReadOnlyList<SeatType>> GetPageAsync(SeatTypeQuery query);
        /// <summary>All seat types, soft-deleted ones included (old tickets still show them).</summary>
        Task<IReadOnlyList<SeatType>> GetAllAsync();
        Task<IReadOnlyList<SeatType>> GetByIdsAsync(IEnumerable<int> ids);
        Task<bool> ActiveNameOrColorExistsAsync(string name, string seatColor, int? excludedId = null);
        /// <summary>True when any seat uses this type, so it can only be soft-deleted.</summary>
        Task<bool> IsUsedAsync(int id);
    }
}
