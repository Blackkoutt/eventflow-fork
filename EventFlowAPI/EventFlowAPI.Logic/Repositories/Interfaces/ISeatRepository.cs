using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    /// <summary>Seats belong to halls and are changed through IHallRepository; this repository only reads them.</summary>
    public interface ISeatRepository : IRepository
    {
        /// <summary>Seats with their hall, seat type and reservations (with tickets).</summary>
        Task<IReadOnlyList<Seat>> GetByIdsAsync(IEnumerable<int> ids);
        Task<bool> ExistsAsync(int id);
    }
}
