using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class PartnerRepository(APIContext context) : EntityRepository<Partner>(context), IPartnerRepository
    {
        public Task<IReadOnlyList<Partner>> GetPageAsync(PartnerQuery query)
        {
            var partners = Table.Where(p => !p.IsDeleted);
            if (query.Kind != null)
            {
                partners = partners.Where(p => p.Kind == query.Kind);
            }
            return ToListAsync(partners.ByName(query)
                                       .SortBy(query.SortBy, query.SortDirection)
                                       .GetPage(query.PageNumber, query.PageSize));
        }

        public Task<IReadOnlyList<Partner>> GetByKindAsync(PartnerKind kind)
            => ToListAsync(Table.Where(p => p.Kind == kind));

        public Task<IReadOnlyList<Partner>> GetActiveByIdsAsync(PartnerKind kind, IEnumerable<int> ids)
        {
            var idList = ids.Distinct().ToList();
            return ToListAsync(Table.Where(p => !p.IsDeleted && p.Kind == kind && idList.Contains(p.Id)));
        }

        public Task<bool> ActiveNameExistsAsync(PartnerKind? kind, string name, int? excludedId = null)
            => Table.AnyAsync(p => p.Id != excludedId && p.Kind == kind && !p.IsDeleted && p.Name.ToLower() == name.ToLower());

        public Task<bool> IsUsedAsync(int id)
            => _context.Festival_Partner.AnyAsync(fp => fp.PartnerId == id);

        public async Task<IReadOnlyList<(string Name, int FestivalCount)>> CountFestivalsAsync(PartnerKind kind, IEnumerable<int> festivalIds)
        {
            var idList = festivalIds.Distinct().ToList();
            var counts = await Table.Where(p => p.Kind == kind)
                                    .Select(p => new { p.Name, FestivalCount = p.Festivals.Count(f => idList.Contains(f.Id)) })
                                    .Where(p => p.FestivalCount > 0)
                                    .ToListAsync();
            return counts.Select(p => (p.Name, p.FestivalCount)).ToList();
        }
    }
}
