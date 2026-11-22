using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class EventPassTypeRepository(APIContext context) : EntityRepository<EventPassType>(context), IEventPassTypeRepository
    {
        public Task<IReadOnlyList<EventPassType>> GetPageAsync(EventPassTypeQuery query)
            => ToListAsync(Table.Where(ept => !ept.IsDeleted)
                                .ByName(query)
                                .ByPrice(query)
                                .SortBy(query.SortBy, query.SortDirection)
                                .GetPage(query.PageNumber, query.PageSize));

        public Task<IReadOnlyList<EventPassType>> GetAllAsync() => ToListAsync(Table);

        public Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null)
            => Table.AnyAsync(ept => ept.Id != excludedId && !ept.IsDeleted && ept.Name.ToLower() == name.ToLower());

        public Task<bool> IsUsedAsync(int id)
            => _context.EventPass.AnyAsync(ep => ep.PassTypeId == id);
    }
}
