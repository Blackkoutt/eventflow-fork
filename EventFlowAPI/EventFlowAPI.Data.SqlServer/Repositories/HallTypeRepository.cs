using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class HallTypeRepository(APIContext context) : EntityRepository<HallType>(context), IHallTypeRepository
    {
        private IQueryable<HallType> WithEquipment => Table.Include(ht => ht.Equipments);

        public override Task<HallType?> GetByIdAsync(int id)
            => WithEquipment.FirstOrDefaultAsync(ht => ht.Id == id);

        public Task<IReadOnlyList<HallType>> GetPageAsync(HallTypeQuery query)
            => ToListAsync(WithEquipment.Where(ht => !ht.IsDeleted)
                                        .ByName(query)
                                        .SortBy(query.SortBy, query.SortDirection)
                                        .GetPage(query.PageNumber, query.PageSize));

        public Task<bool> ActiveNameExistsAsync(string name, int? excludedId = null)
            => Table.AnyAsync(ht => ht.Id != excludedId && !ht.IsDeleted && ht.Name.ToLower() == name.ToLower());

        public Task<bool> IsUsedAsync(int id)
            => _context.Hall.AnyAsync(h => h.HallTypeId == id);
    }
}
