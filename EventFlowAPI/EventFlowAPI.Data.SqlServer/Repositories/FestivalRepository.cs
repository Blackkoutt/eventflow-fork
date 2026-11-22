using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class FestivalRepository(APIContext context) : EntityRepository<Festival>(context), IFestivalRepository
    {
        private IQueryable<Festival> WithDetails => Table.Include(f => f.Events.OrderBy(e => e.Id))
                                                             .ThenInclude(e => e.Category)
                                                         .Include(f => f.Events.OrderBy(e => e.Id))
                                                             .ThenInclude(e => e.Hall)
                                                         .Include(f => f.Partners)
                                                         .Include(f => f.Tickets)
                                                             .ThenInclude(t => t.TicketType)
                                                         .AsSplitQuery();

        public override Task<Festival?> GetByIdAsync(int id)
            => WithDetails.FirstOrDefaultAsync(f => f.Id == id);

        public Task<IReadOnlyList<Festival>> GetPageAsync(FestivalQuery query)
            => ToListAsync(WithDetails.ByQuery(query).GetPage(query.PageNumber, query.PageSize));

        public Task<IReadOnlyList<Festival>> GetActiveWithAnyEventAsync(IEnumerable<int> eventIds)
        {
            var idList = eventIds.Distinct().ToList();
            return ToListAsync(WithDetails.Where(f => !f.IsDeleted &&
                                                      f.EndDate > DateTime.Now &&
                                                      f.Events.Any(e => idList.Contains(e.Id))));
        }

        public Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null)
            => Table.AnyAsync(f => f.Id != excludedId && !f.IsDeleted && f.EndDate >= DateTime.Now &&
                                   f.Name.ToLower() == name.ToLower());

        public Task<IReadOnlyList<Festival>> GetAllAsync() => ToListAsync(WithDetails);
    }
}
