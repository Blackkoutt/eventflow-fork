using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    // TODO (block 6): GenericRepository stays only because StatisticsService still uses GetRepository<HallRent>()
    public sealed class HallRentRepository(APIContext context) : GenericRepository<HallRent>(context), IHallRentRepository
    {
        private IQueryable<HallRent> WithDetails => _context.HallRent
                                                        .Include(hr => hr.User)
                                                        .Include(hr => hr.Hall)
                                                            .ThenInclude(h => h.Type)
                                                                .ThenInclude(t => t.Equipments)
                                                        .Include(hr => hr.AdditionalServices)
                                                        .AsSplitQuery();

        private IQueryable<HallRent> Active => WithDetails.Where(hr => !hr.IsDeleted && hr.EndDate > DateTime.Now);

        public Task<HallRent?> GetByIdAsync(int id) => WithDetails.FirstOrDefaultAsync(hr => hr.Id == id);
        public override Task<HallRent?> GetOneAsync(int id) => GetByIdAsync(id);
        public override async Task<IEnumerable<HallRent>> GetAllAsync(Func<IQueryable<HallRent>, IQueryable<HallRent>>? query = null)
            => await (query?.Invoke(WithDetails) ?? WithDetails).ToListAsync();

        public Task<IReadOnlyList<HallRent>> GetPageAsync(HallRentQuery query)
            => ToListAsync(WithDetails.ByQuery(query).GetPage(query.PageNumber, query.PageSize));

        public Task<IReadOnlyList<HallRent>> GetUserPageAsync(HallRentQuery query, string userId)
            => ToListAsync(WithDetails.ByStatus(query.Status)
                                      .Where(hr => hr.UserId == userId)
                                      .SortBy(query.SortBy, query.SortDirection)
                                      .GetPage(query.PageNumber, query.PageSize));

        public Task<int> CountActiveForUserStartingThisMonthAsync(string userId)
            => _context.HallRent.CountAsync(hr => hr.UserId == userId &&
                                                  !hr.IsDeleted &&
                                                  hr.EndDate > DateTime.Now &&
                                                  hr.StartDate.Month == DateTime.Now.Month);

        public Task<IReadOnlyList<HallRent>> GetActiveForDefaultHallAsync(int? defaultHallId)
            => ToListAsync(Active.Where(hr => hr.Hall.DefaultId == defaultHallId));

        public Task<IReadOnlyList<HallRent>> GetStartingAfterInHallsAsync(IEnumerable<int> hallIds, DateTime after)
        {
            var idList = hallIds.Distinct().ToList();
            return ToListAsync(WithDetails.Where(hr => !hr.IsDeleted && hr.StartDate > after && idList.Contains(hr.HallId)));
        }

        public Task<bool> HasCollisionAsync(int defaultHallId, DateTime startDate, DateTime endDate, int? excludedHallRentId = null)
            => _context.HallRent.AnyAsync(hr => hr.Id != excludedHallRentId &&
                                                !hr.IsDeleted &&
                                                hr.EndDate >= DateTime.Now &&
                                                hr.Hall.DefaultId == defaultHallId &&
                                                startDate < hr.EndDate &&
                                                endDate > hr.StartDate);

        private static async Task<IReadOnlyList<HallRent>> ToListAsync(IQueryable<HallRent> query) => await query.ToListAsync();
    }
}
