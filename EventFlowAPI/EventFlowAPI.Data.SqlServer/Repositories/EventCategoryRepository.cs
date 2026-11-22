using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class EventCategoryRepository(APIContext context) : EntityRepository<EventCategory>(context), IEventCategoryRepository
    {
        public Task<IReadOnlyList<EventCategory>> GetPageAsync(EventCategoryQuery query)
            => ToListAsync(Table.Where(ec => !ec.IsDeleted)
                                .ByName(query)
                                .SortBy(query.SortBy, query.SortDirection)
                                .GetPage(query.PageNumber, query.PageSize));

        public Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null)
            => Table.AnyAsync(ec => ec.Id != excludedId && !ec.IsDeleted && ec.Name.ToLower() == name.ToLower());

        public Task<bool> IsUsedAsync(int id)
            => _context.Event.AnyAsync(e => e.CategoryId == id);
    }
}
