using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class SeatRepository(APIContext context) : GenericRepository<Seat>(context), ISeatRepository
    {
        public sealed override async Task<IEnumerable<Seat>> GetAllAsync(Func<IQueryable<Seat>, IQueryable<Seat>>? query = null)
        {
            var _table = _context.Seat
                                .Include(s => s.Hall)
                                .Include(s => s.SeatType)
                                .Include(s => s.Reservations)
                                    .ThenInclude(r => r.Ticket)
                                .AsSplitQuery();

            return await (query != null ? query(_table).ToListAsync() : _table.ToListAsync());
        }

        public sealed override async Task<Seat?> GetOneAsync(int id)
        {
            return await _context.Seat
                        .AsSplitQuery()
                        .Include(s => s.Hall)
                        .Include(s => s.SeatType)
                        .Include(s => s.Reservations)
                            .ThenInclude(r => r.Ticket)
                        .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
