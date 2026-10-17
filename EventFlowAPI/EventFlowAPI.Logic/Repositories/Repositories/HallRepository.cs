using EventFlowAPI.DB.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Logic.Repositories.Repositories
{
    public sealed class HallRepository(APIContext context) : GenericRepository<Hall>(context), IHallRepository
    {
        public sealed override async Task<IEnumerable<Hall>> GetAllAsync(Func<IQueryable<Hall>, IQueryable<Hall>>? query = null)
        {
            var _table = _context.Hall
                        .Include(h => h.Type)
                            .ThenInclude(t => t.Equipments)
                        .Include(h => h.Seats)
                            .ThenInclude(s => s.SeatType)
                        .Include(h => h.Seats)
                            .ThenInclude(s => s.Reservations);

            return await (query != null ? query(_table).ToListAsync() : _table.ToListAsync());
        }

        public sealed override async Task<Hall?> GetOneAsync(int id)
        {
            return await _context.Hall
                        .Include(h => h.Type)
                            .ThenInclude(t => t.Equipments)
                        .Include(h => h.Seats)
                            .ThenInclude(s => s.SeatType)
                        .Include(h => h.Seats)
                            .ThenInclude(s => s.Reservations)
                        .FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}
