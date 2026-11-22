using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    // TODO (block 6): GenericRepository stays only because StatisticsService still uses GetRepository<Reservation>()
    public sealed class ReservationRepository(APIContext context) : GenericRepository<Reservation>(context), IReservationRepository
    {
        private IQueryable<Reservation> WithDetails => _context.Reservation
                                                        .Include(r => r.User)
                                                        .Include(r => r.Seats)
                                                            .ThenInclude(s => s.SeatType)
                                                        .Include(r => r.Ticket)
                                                            .ThenInclude(t => t.TicketType)
                                                        .Include(r => r.Ticket)
                                                            .ThenInclude(t => t.Event)
                                                                .ThenInclude(e => e.Hall)
                                                        .Include(r => r.Ticket)
                                                            .ThenInclude(t => t.Event)
                                                                .ThenInclude(e => e.Category)
                                                        .Include(r => r.Ticket)
                                                            .ThenInclude(t => t != null ? t.Festival : null)
                                                        .AsSplitQuery();

        private IQueryable<Reservation> Active => WithDetails.Where(r => !r.IsDeleted && r.EndDate > DateTime.Now);

        public Task<Reservation?> GetByIdAsync(int id) => WithDetails.FirstOrDefaultAsync(r => r.Id == id);
        public override Task<Reservation?> GetOneAsync(int id) => GetByIdAsync(id);
        public override async Task<IEnumerable<Reservation>> GetAllAsync(Func<IQueryable<Reservation>, IQueryable<Reservation>>? query = null)
            => await (query?.Invoke(WithDetails) ?? WithDetails).ToListAsync();

        public Task<IReadOnlyList<Reservation>> GetPageAsync(ReservationQuery query, string? userId = null)
        {
            var reservations = WithDetails.ByQuery(query);
            if (userId != null)
            {
                reservations = reservations.Where(r => r.UserId == userId);
            }
            return ToListAsync(reservations.GetPage(query.PageNumber, query.PageSize));
        }

        public Task<IReadOnlyList<Reservation>> GetByGuidAsync(Guid reservationGuid)
            => ToListAsync(WithDetails.Where(r => r.ReservationGuid == reservationGuid));

        public Task<IReadOnlyList<Reservation>> GetOtherFestivalReservationsAsync(int excludedReservationId, string userId, int festivalId)
            => ToListAsync(WithDetails.Where(r => r.Id != excludedReservationId &&
                                                  r.UserId == userId &&
                                                  r.Ticket.FestivalId == festivalId));

        public Task<IReadOnlyList<Reservation>> GetActiveForEventAsync(int eventId)
            => ToListAsync(Active.Where(r => r.Ticket.EventId == eventId));

        public Task<IReadOnlyList<Reservation>> GetActiveForFestivalAsync(int festivalId)
            => ToListAsync(Active.Where(r => r.Ticket.FestivalId == festivalId));

        public Task<IReadOnlyList<Reservation>> GetFinishedForFestivalAsync(int festivalId)
            => ToListAsync(WithDetails.Where(r => r.EndDate < DateTime.Now && !r.IsDeleted && r.Ticket.FestivalId == festivalId));

        public Task<IReadOnlyList<Reservation>> GetActiveInHallsAsync(IEnumerable<int> hallIds)
        {
            var idList = hallIds.Distinct().ToList();
            return ToListAsync(Active.Where(r => idList.Contains(r.Ticket.Event.HallId)));
        }

        public Task<IReadOnlyList<Reservation>> GetActiveForDefaultHallAsync(int? defaultHallId)
            => ToListAsync(Active.Where(r => r.Ticket.Event.Hall.DefaultId == defaultHallId));

        public Task<IReadOnlyList<Reservation>> GetActiveForEventPassAsync(int eventPassId)
            => ToListAsync(Active.Where(r => r.EventPassId == eventPassId));

        private static async Task<IReadOnlyList<Reservation>> ToListAsync(IQueryable<Reservation> query) => await query.ToListAsync();
    }
}
