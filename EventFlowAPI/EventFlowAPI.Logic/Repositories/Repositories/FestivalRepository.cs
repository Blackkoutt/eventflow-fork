using EventFlowAPI.DB.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Logic.Repositories.Repositories
{
    public sealed class FestivalRepository(APIContext context) : GenericRepository<Festival>(context), IFestivalRepository
    {
        public sealed override async Task<IEnumerable<Festival>> GetAllAsync(Func<IQueryable<Festival>, IQueryable<Festival>>? query = null)
        {
            var _table = _context.Festival
                        .Include(f => f.Details)
                        .Include(f => f.Events.OrderBy(e => e.Id))
                            .ThenInclude(e => e.Category)
                        .Include(f => f.Events.OrderBy(e => e.Id))
                            .ThenInclude(e => e.Hall)
                        .Include(f => f.Partners)
                        .Include(f => f.Tickets)
                            .ThenInclude(t => t.TicketType)
                        .AsSplitQuery();

            return await (query != null ? query(_table).ToListAsync() : _table.ToListAsync());
        }

        public sealed override async Task<Festival?> GetOneAsync(int id)
        {
            return await _context.Festival
                        .AsSplitQuery()
                        .Include(f => f.Details)
                        .Include(f => f.Events.OrderBy(e => e.Id))
                            .ThenInclude(e => e.Category)
                        .Include(f => f.Events.OrderBy(e => e.Id))
                            .ThenInclude(e => e.Hall)
                        .Include(f => f.Partners)
                        .Include(f => f.Tickets)
                            .ThenInclude(t => t.TicketType)
                        .FirstOrDefaultAsync(f => f.Id == id);
        }
    }
}
