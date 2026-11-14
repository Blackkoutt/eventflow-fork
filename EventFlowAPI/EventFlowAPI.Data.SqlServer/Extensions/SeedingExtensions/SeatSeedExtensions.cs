using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class SeatSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<Seat> entityBuilder)
        {
            entityBuilder.HasData(
                new Seat
                {
                    Id = 1,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 1,
                    HallId = 1
                },
                new Seat
                {
                    Id = 2,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 1,
                    HallId = 1
                },
                new Seat
                {
                    Id = 3,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 1
                },
                new Seat
                {
                    Id = 4,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 1,
                    HallId = 1
                },
                new Seat
                {
                    Id = 5,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 2,
                    HallId = 2
                },
                new Seat
                {
                    Id = 6,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 2,
                    HallId = 2
                },
                new Seat
                {
                    Id = 7,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 2
                },
                new Seat
                {
                    Id = 8,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 2,
                    HallId = 2
                },
                new Seat
                {
                    Id = 9,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 3
                },
                new Seat
                {
                    Id = 10,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 3,
                    HallId = 3
                },
                new Seat
                {
                    Id = 11,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 3,
                    HallId = 3
                },
                new Seat
                {
                    Id = 12,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 3,
                    HallId = 3
                },
                new Seat
                {
                    Id = 13,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 4
                },
                 new Seat
                 {
                     Id = 14,
                     SeatNr = 1,
                     Row = 1,
                     GridRow = 1,
                     Column = 1,
                     GridColumn = 1,
                     SeatTypeId = 1,
                     HallId = 5
                 },
                new Seat
                {
                    Id = 15,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 1,
                    HallId = 5
                },
                new Seat
                {
                    Id = 16,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 5
                },
                new Seat
                {
                    Id = 17,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 1,
                    HallId = 5
                },
                 new Seat
                 {
                     Id = 18,
                     SeatNr = 1,
                     Row = 1,
                     GridRow = 1,
                     Column = 1,
                     GridColumn = 1,
                     SeatTypeId = 1,
                     HallId = 9
                 },
                new Seat
                {
                    Id = 19,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 1,
                    HallId = 9
                },
                new Seat
                {
                    Id = 20,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 9
                },
                new Seat
                {
                    Id = 21,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 1,
                    HallId = 9
                },
                 new Seat
                 {
                     Id = 22,
                     SeatNr = 1,
                     Row = 1,
                     GridRow = 1,
                     Column = 1,
                     GridColumn = 1,
                     SeatTypeId = 1,
                     HallId = 12
                 },
                new Seat
                {
                    Id = 23,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 1,
                    HallId = 12
                },
                new Seat
                {
                    Id = 24,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 12
                },
                new Seat
                {
                    Id = 25,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 1,
                    HallId = 12
                },
                new Seat
                {
                    Id = 26,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 2,
                    HallId = 6
                },
                new Seat
                {
                    Id = 27,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 2,
                    HallId = 6
                },
                new Seat
                {
                    Id = 28,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 6
                },
                new Seat
                {
                    Id = 29,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 2,
                    HallId = 6
                },
                new Seat
                {
                    Id = 30,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 2,
                    HallId = 10
                },
                new Seat
                {
                    Id = 31,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 2,
                    HallId = 10
                },
                new Seat
                {
                    Id = 32,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 10
                },
                new Seat
                {
                    Id = 33,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 2,
                    HallId = 10
                },
                new Seat
                {
                    Id = 34,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 2,
                    HallId = 13
                },
                new Seat
                {
                    Id = 35,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 2,
                    HallId = 13
                },
                new Seat
                {
                    Id = 36,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 13
                },
                new Seat
                {
                    Id = 37,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 2,
                    HallId = 13
                },
                new Seat
                {
                    Id = 38,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 7
                },
                new Seat
                {
                    Id = 39,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 3,
                    HallId = 7
                },
                new Seat
                {
                    Id = 40,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 3,
                    HallId = 7
                },
                new Seat
                {
                    Id = 41,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 3,
                    HallId = 7
                },
                new Seat
                {
                    Id = 42,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 14
                },
                new Seat
                {
                    Id = 43,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 3,
                    HallId = 14
                },
                new Seat
                {
                    Id = 44,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 3,
                    HallId = 14
                },
                new Seat
                {
                    Id = 45,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 3,
                    HallId = 14
                },
                new Seat
                {
                    Id = 46,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 8
                },
                new Seat
                {
                    Id = 47,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 11
                },
                new Seat
                {
                    Id = 48,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 15
                },
                new Seat
                {
                    Id = 49,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 16
                },
                new Seat
                {
                    Id = 50,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 3,
                    HallId = 16
                },
                new Seat
                {
                    Id = 51,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 3,
                    HallId = 16
                },
                new Seat
                {
                    Id = 52,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 3,
                    HallId = 16
                },
                 new Seat
                 {
                     Id = 53,
                     SeatNr = 1,
                     Row = 1,
                     GridRow = 1,
                     Column = 1,
                     GridColumn = 1,
                     SeatTypeId = 2,
                     HallId = 17
                 },
                new Seat
                {
                    Id = 54,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 2,
                    HallId = 17
                },
                new Seat
                {
                    Id = 55,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 17
                },
                new Seat
                {
                    Id = 56,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 2,
                    HallId = 17
                },
                new Seat
                {
                    Id = 57,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 1,
                    HallId = 18
                },
                new Seat
                {
                    Id = 58,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 1,
                    HallId = 18
                },
                new Seat
                {
                    Id = 59,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 1,
                    HallId = 18
                },
                new Seat
                {
                    Id = 60,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 1,
                    HallId = 18
                },
                new Seat
                {
                    Id = 61,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 19
                },
                new Seat
                {
                    Id = 62,
                    SeatNr = 2,
                    Row = 1,
                    GridRow = 1,
                    Column = 2,
                    GridColumn = 2,
                    SeatTypeId = 3,
                    HallId = 19
                },
                new Seat
                {
                    Id = 63,
                    SeatNr = 3,
                    Row = 1,
                    GridRow = 1,
                    Column = 3,
                    GridColumn = 3,
                    SeatTypeId = 3,
                    HallId = 19
                },
                new Seat
                {
                    Id = 64,
                    SeatNr = 4,
                    Row = 1,
                    GridRow = 1,
                    Column = 4,
                    GridColumn = 4,
                    SeatTypeId = 3,
                    HallId = 19
                },
                new Seat
                {
                    Id = 65,
                    SeatNr = 1,
                    Row = 1,
                    GridRow = 1,
                    Column = 1,
                    GridColumn = 1,
                    SeatTypeId = 3,
                    HallId = 20
                }
            );
        }
    }
}
