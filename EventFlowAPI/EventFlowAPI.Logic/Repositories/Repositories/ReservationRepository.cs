using EventFlowAPI.DB.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Repositories.Interfaces.BaseInterfaces;
using EventFlowAPI.Logic.Repositories.Repositories.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Logic.Repositories.Repositories
{
    public sealed class ReservationRepository(APIContext context) : GenericRepository<Reservation>(context), IReservationRepository
    {
        public sealed override async Task<IEnumerable<Reservation>> GetAllAsync(Func<IQueryable<Reservation>, IQueryable<Reservation>>? query = null)
        {
            var _table = _context.Reservation
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

            return await (query != null ? query(_table).ToListAsync() : _table.ToListAsync());
        }

        public sealed override async Task<Reservation?> GetOneAsync(int id)
        {
            return await _context.Reservation
                         .AsSplitQuery()
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
                        .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
