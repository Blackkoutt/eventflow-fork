using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class SeatRepository(APIContext context) : ISeatRepository
    {
        public async Task<IReadOnlyList<Seat>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.Distinct().ToList();
            return await context.Seat
                                .Include(s => s.Hall)
                                .Include(s => s.SeatType)
                                .Include(s => s.Reservations)
                                    .ThenInclude(r => r.Ticket)
                                .AsSplitQuery()
                                .Where(s => idList.Contains(s.Id))
                                .ToListAsync();
        }

        public Task<bool> ExistsAsync(int id) => context.Seat.AnyAsync(s => s.Id == id);
    }
}
