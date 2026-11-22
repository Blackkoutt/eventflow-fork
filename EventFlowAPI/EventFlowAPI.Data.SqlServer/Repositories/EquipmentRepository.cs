using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class EquipmentRepository(APIContext context) : EntityRepository<Equipment>(context), IEquipmentRepository
    {
        public Task<IReadOnlyList<Equipment>> GetPageAsync(EquipmentQuery query)
            => ToListAsync(Table.ByName(query)
                                .SortBy(query.SortBy, query.SortDirection)
                                .GetPage(query.PageNumber, query.PageSize));

        public Task<IReadOnlyList<Equipment>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.Distinct().ToList();
            return ToListAsync(Table.Where(e => idList.Contains(e.Id)));
        }

        public Task<bool> NameExistsAsync(string name, int? excludedId = null)
            => Table.AnyAsync(e => e.Id != excludedId && e.Name.ToLower() == name.ToLower());
    }
}
