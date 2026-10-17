using EventFlowAPI.DB.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Logic.Repositories.Repositories
{
    public sealed class EventRepository(APIContext context) : GenericRepository<Event>(context), IEventRepository
    {
        public sealed override async Task<IEnumerable<Event>> GetAllAsync(Func<IQueryable<Event>, IQueryable<Event>>? query = null)
        {
            var _table = _context.Event
                        .Include(e => e.Hall)
                        .Include(e => e.Category)
                        .Include(e => e.Tickets)
                            .ThenInclude(t => t.TicketType)
                        .Include(e => e.Festivals);

            return await (query != null ? query(_table).ToListAsync() : _table.ToListAsync());
        }
        public sealed override async Task<Event?> GetOneAsync(int id)
        {
            return await _context.Event
                        .Include(e => e.Hall)
                        .Include(e => e.Category)
                        .Include(e => e.Festivals)
                        .Include(e => e.Tickets)
                            .ThenInclude(t => t.TicketType)
                        .FirstOrDefaultAsync(e=> e.Id == id);
        }
    }
}
