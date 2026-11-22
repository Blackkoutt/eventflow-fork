using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Data.SqlServer.Repositories.BaseRepositories;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Data.SqlServer.Repositories
{
    public sealed class HallRepository(APIContext context) : EntityRepository<Hall>(context), IHallRepository
    {
        // TODO (block 5): seat reservations are loaded only because services still read Seat.Reservations
        private IQueryable<Hall> WithDetails => Table.Include(h => h.Type)
                                                         .ThenInclude(t => t.Equipments)
                                                     .Include(h => h.Seats)
                                                         .ThenInclude(s => s.SeatType)
                                                     .Include(h => h.Seats)
                                                         .ThenInclude(s => s.Reservations);

        public override Task<Hall?> GetByIdAsync(int id)
            => WithDetails.FirstOrDefaultAsync(h => h.Id == id);

        public Task<IReadOnlyList<Hall>> GetPageAsync(HallQuery query)
            => ToListAsync(WithDetails.Where(h => h.IsVisible)
                                      .ByQuery(query)
                                      .GetPage(query.PageNumber, query.PageSize));

        public Task<bool> VisibleHallNrExistsAsync(int hallNr, int? excludedId = null)
            => Table.AnyAsync(h => h.Id != excludedId && h.IsVisible && h.HallNr == hallNr);

        public async Task<int?> GetDefaultIdAsync(int hallId)
        {
            var hall = await Table.Where(h => h.Id == hallId)
                                  .Select(h => new { h.DefaultId })
                                  .FirstOrDefaultAsync();
            return hall?.DefaultId;
        }

        public Task<IReadOnlyList<Hall>> GetCopiesAsync(int defaultHallId, int excludedId)
            => ToListAsync(WithDetails.Where(h => h.Id != excludedId && h.DefaultId == defaultHallId));

        public async Task<Hall> AddCopyAsync(Hall source)
        {
            // The copy reuses the loaded object: stop tracking it as the original, then insert it as a new row.
            // Its type and seat types stay tracked, so they are referenced and not inserted again.
            _context.Entry(source).State = EntityState.Detached;
            source.Id = 0;
            source.Seats = source.Seats.Select(seat => new Seat
            {
                SeatNr = seat.SeatNr,
                Row = seat.Row,
                GridRow = seat.GridRow,
                Column = seat.Column,
                GridColumn = seat.GridColumn,
                SeatTypeId = seat.SeatTypeId,
            }).ToList();
            source.Events = [];
            source.Rents = [];
            source.IsCopy = true;
            source.IsVisible = false;

            await Table.AddAsync(source);
            return source;
        }

        public void DeleteSeats(Hall hall) => _context.Seat.RemoveRange(hall.Seats);
    }
}
