using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class AdditionalServicesRepository(APIContext context) : EntityRepository<AdditionalServices>(context), IAdditionalServicesRepository
    {
        public Task<IReadOnlyList<AdditionalServices>> GetPageAsync(AdditionalServicesQuery query)
            => ToListAsync(Table.Where(s => !s.IsDeleted)
                                .ByName(query)
                                .ByPrice(query)
                                .SortBy(query.SortBy, query.SortDirection)
                                .GetPage(query.PageNumber, query.PageSize));

        public Task<IReadOnlyList<AdditionalServices>> GetAllAsync() => ToListAsync(Table);

        public Task<IReadOnlyList<AdditionalServices>> GetActiveByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.Distinct().ToList();
            return ToListAsync(Table.Where(s => !s.IsDeleted && idList.Contains(s.Id)));
        }

        public Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null)
            => Table.AnyAsync(s => s.Id != excludedId && !s.IsDeleted && s.Name.ToLower() == name.ToLower());

        public Task<bool> IsUsedAsync(int id)
            => _context.HallRent_AdditionalServices.AnyAsync(ras => ras.AdditionalServicesId == id);
    }
}
