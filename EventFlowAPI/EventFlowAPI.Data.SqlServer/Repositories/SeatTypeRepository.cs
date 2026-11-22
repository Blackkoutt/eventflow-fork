using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class SeatTypeRepository(APIContext context) : EntityRepository<SeatType>(context), ISeatTypeRepository
    {
        public Task<IReadOnlyList<SeatType>> GetPageAsync(SeatTypeQuery query)
            => ToListAsync(Table.Where(st => !st.IsDeleted)
                                .ByName(query)
                                .SortBy(query.SortBy, query.SortDirection)
                                .GetPage(query.PageNumber, query.PageSize));

        public Task<IReadOnlyList<SeatType>> GetAllAsync() => ToListAsync(Table);

        public Task<IReadOnlyList<SeatType>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.Distinct().ToList();
            return ToListAsync(Table.Where(st => idList.Contains(st.Id)));
        }

        public Task<bool> ActiveNameOrColorExistsAsync(string name, string seatColor, int? excludedId = null)
            => Table.AnyAsync(st => st.Id != excludedId && !st.IsDeleted &&
                                    (st.Name.ToLower() == name.ToLower() || st.SeatColor == seatColor));

        public Task<bool> IsUsedAsync(int id)
            => _context.Seat.AnyAsync(s => s.SeatTypeId == id);
    }
}
