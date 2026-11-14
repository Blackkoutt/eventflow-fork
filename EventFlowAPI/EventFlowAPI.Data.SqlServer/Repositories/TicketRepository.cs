using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class TicketRepository(APIContext context) : GenericRepository<Ticket>(context), ITicketRepository
    {
        public sealed override async Task<IEnumerable<Ticket>> GetAllAsync(Func<IQueryable<Ticket>, IQueryable<Ticket>>? query = null)
        {
            var _table = _context.Ticket
                        .Include(t => t.TicketType)
                        .Include(t => t.Event)
                            .ThenInclude(e => e.Category)
                        .Include(t => t.Event)
                            .ThenInclude(e => e.Hall)
                        .Include(t => t.Festival)
                            .ThenInclude(f => f != null ? f.Events : null)
                        .AsSplitQuery();

            return await (query != null ? query(_table).ToListAsync() : _table.ToListAsync());
        }
        public sealed override async Task<Ticket?> GetOneAsync(int id)
        {
            return await _context.Ticket
                        .AsSplitQuery()
                        .Include(t => t.TicketType)
                        .Include(t => t.Event)
                            .ThenInclude(e => e.Category)
                        .Include(t => t.Event)
                            .ThenInclude(e => e.Hall)
                        .Include(t => t.Festival)
                            .ThenInclude(f => f != null ? f.Events : null)
                        .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
