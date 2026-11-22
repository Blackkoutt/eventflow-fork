using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Repositories.Interfaces
{
    /// <summary>
    /// A hall is one aggregate with its seats. Every event and hall rent works on its own copy
    /// of a hall (IsCopy, DefaultId = id of the original), so later changes of the original do not move booked seats.
    /// Halls are returned with their type, equipment, seats and seat types.
    /// </summary>
    public interface IHallRepository : IEntityRepository<Hall>
    {
        Task<IReadOnlyList<Hall>> GetPageAsync(HallQuery query);
        Task<bool> VisibleHallNrExistsAsync(int hallNr, int? excludedId = null);
        /// <summary>DefaultId of the hall (the id of its original), or null when the hall does not exist.</summary>
        Task<int?> GetDefaultIdAsync(int hallId);
        /// <summary>Copies made from the original hall, without the hall with excludedId.</summary>
        Task<IReadOnlyList<Hall>> GetCopiesAsync(int defaultHallId, int excludedId);
        /// <summary>Adds a hidden copy of the hall with new seats; it gets an id on SaveChangesAsync.</summary>
        Task<Hall> AddCopyAsync(Hall source);
        /// <summary>Removes the seats of a hall copy that is no longer used.</summary>
        void DeleteSeats(Hall hall);
    }
}
