using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    // TODO (block 6): GenericRepository stays only because StatisticsService still uses GetRepository<EventPass>()
    public sealed class EventPassRepository(APIContext context) : GenericRepository<EventPass>(context), IEventPassRepository
    {
        private IQueryable<EventPass> WithDetails => _context.EventPass
                                                        .Include(ep => ep.User)
                                                        .Include(ep => ep.PassType)
                                                        .AsSplitQuery();

        public Task<EventPass?> GetByIdAsync(int id) => WithDetails.FirstOrDefaultAsync(ep => ep.Id == id);
        public override Task<EventPass?> GetOneAsync(int id) => GetByIdAsync(id);
        public override async Task<IEnumerable<EventPass>> GetAllAsync(Func<IQueryable<EventPass>, IQueryable<EventPass>>? query = null)
            => await (query?.Invoke(WithDetails) ?? WithDetails).ToListAsync();

        public async Task<IReadOnlyList<EventPass>> GetPageAsync(EventPassQuery query, string? userId = null)
        {
            var passes = WithDetails.ByQuery(query);
            if (userId != null)
            {
                passes = passes.Where(ep => ep.UserId == userId);
            }
            return await passes.GetPage(query.PageNumber, query.PageSize).ToListAsync();
        }

        public Task<EventPass?> GetActiveForUserAsync(string userId)
            => WithDetails.FirstOrDefaultAsync(ep => ep.UserId == userId && !ep.IsDeleted && ep.EndDate > DateTime.Now);
    }
}
