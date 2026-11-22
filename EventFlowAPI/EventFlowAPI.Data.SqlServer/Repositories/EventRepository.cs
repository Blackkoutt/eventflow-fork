using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class EventRepository(APIContext context) : EntityRepository<Event>(context), IEventRepository
    {
        private IQueryable<Event> WithDetails => Table.Include(e => e.Hall)
                                                      .Include(e => e.Category)
                                                      .Include(e => e.Festivals)
                                                      .Include(e => e.Tickets)
                                                          .ThenInclude(t => t.TicketType);

        private IQueryable<Event> Active => WithDetails.Where(e => !e.IsDeleted && e.EndDate > DateTime.Now);

        public override Task<Event?> GetByIdAsync(int id)
            => WithDetails.FirstOrDefaultAsync(e => e.Id == id);

        public Task<IReadOnlyList<Event>> GetPageAsync(EventQuery query)
            => ToListAsync(WithDetails.ByQuery(query).GetPage(query.PageNumber, query.PageSize));

        public Task<IReadOnlyList<Event>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.Distinct().ToList();
            return ToListAsync(WithDetails.Where(e => idList.Contains(e.Id)));
        }

        public Task<IReadOnlyList<Event>> GetActiveByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.Distinct().ToList();
            return ToListAsync(Active.Where(e => idList.Contains(e.Id)).OrderBy(e => e.StartDate));
        }

        public Task<IReadOnlyList<Event>> GetActiveInHallsAsync(IEnumerable<int> hallIds)
        {
            var idList = hallIds.Distinct().ToList();
            return ToListAsync(Active.Where(e => idList.Contains(e.HallId)));
        }

        public Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null)
            => Table.AnyAsync(e => e.Id != excludedId && !e.IsDeleted && e.EndDate >= DateTime.Now &&
                                   e.Name.ToLower() == name.ToLower());

        public Task<bool> HasCollisionAsync(int defaultHallId, DateTime startDate, DateTime endDate, int? excludedEventId = null)
            => Table.AnyAsync(e => e.Id != excludedEventId &&
                                   !e.IsDeleted &&
                                   e.EndDate >= DateTime.Now &&
                                   e.Hall.DefaultId == defaultHallId &&
                                   startDate < e.EndDate &&
                                   endDate > e.StartDate);

        public Task<IReadOnlyList<Event>> GetAllAsync() => ToListAsync(WithDetails);
    }
}
