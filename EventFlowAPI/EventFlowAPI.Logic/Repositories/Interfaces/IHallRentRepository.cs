using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    /// <summary>
    /// Hall rents are returned with their user, hall copy (with type and equipment) and additional services.
    /// "Active" means not cancelled and not finished yet.
    /// </summary>
    public interface IHallRentRepository : IEntityRepository<HallRent>
    {
        Task<IReadOnlyList<HallRent>> GetPageAsync(HallRentQuery query);
        /// <summary>One page of the user's own rents, filtered by status and sorted.</summary>
        Task<IReadOnlyList<HallRent>> GetUserPageAsync(HallRentQuery query, string userId);
        Task<int> CountActiveForUserStartingThisMonthAsync(string userId);
        /// <summary>Active rents of any copy of the original hall.</summary>
        Task<IReadOnlyList<HallRent>> GetActiveForDefaultHallAsync(int? defaultHallId);
        /// <summary>Not cancelled rents in the given hall copies that start after the given time.</summary>
        Task<IReadOnlyList<HallRent>> GetStartingAfterInHallsAsync(IEnumerable<int> hallIds, DateTime after);
        /// <summary>True when a rent that is not over yet uses a copy of the hall at the same time.</summary>
        Task<bool> HasCollisionAsync(int defaultHallId, DateTime startDate, DateTime endDate, int? excludedHallRentId = null);
    }
}
