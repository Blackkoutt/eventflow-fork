using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class TicketRepository(APIContext context) : EntityRepository<Ticket>(context), ITicketRepository
    {
        private IQueryable<Ticket> WithDetails => Table.Include(t => t.TicketType)
                                                       .Include(t => t.Event)
                                                           .ThenInclude(e => e.Category)
                                                       .Include(t => t.Event)
                                                           .ThenInclude(e => e.Hall)
                                                       .Include(t => t.Festival)
                                                           .ThenInclude(f => f != null ? f.Events : null)
                                                       .AsSplitQuery();

        public override Task<Ticket?> GetByIdAsync(int id)
            => WithDetails.FirstOrDefaultAsync(t => t.Id == id);

        public Task<IReadOnlyList<Ticket>> GetByFestivalAsync(int festivalId)
            => ToListAsync(WithDetails.Where(t => t.FestivalId == festivalId));

        public Task<IReadOnlyList<Ticket>> GetEventTicketsAsync(int eventId)
            => ToListAsync(WithDetails.Where(t => t.FestivalId == null && t.EventId == eventId));

        public Task<IReadOnlyList<Ticket>> GetForEventsOrFestivalsAsync(IEnumerable<int> eventIds, IEnumerable<int> festivalIds)
        {
            var eventIdList = eventIds.Distinct().ToList();
            var festivalIdList = festivalIds.Select(id => (int?)id).Distinct().ToList();
            return ToListAsync(WithDetails.Where(t => (!t.IsDeleted && eventIdList.Contains(t.EventId)) ||
                                                      festivalIdList.Contains(t.FestivalId)));
        }
    }
}
