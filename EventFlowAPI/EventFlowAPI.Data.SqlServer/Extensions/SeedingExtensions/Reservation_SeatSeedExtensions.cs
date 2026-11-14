using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class Reservation_SeatSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<Reservation_Seat> entityBuilder)
        {
            entityBuilder.HasData(
                new Reservation_Seat
                {
                    ReservationId = 8,
                    SeatId = 20
                },
                new Reservation_Seat
                {
                    ReservationId = 3,
                    SeatId = 14
                },
                new Reservation_Seat
                {
                    ReservationId = 1,
                    SeatId = 26
                },
                new Reservation_Seat
                {
                    ReservationId = 5,
                    SeatId = 29
                },
                new Reservation_Seat
                {
                    ReservationId = 6,
                    SeatId = 33
                },
                new Reservation_Seat
                {
                    ReservationId = 2,
                    SeatId = 38
                },
                new Reservation_Seat
                {
                    ReservationId = 7,
                    SeatId = 41
                },
                new Reservation_Seat
                {
                    ReservationId = 4,
                    SeatId = 46
                }
                /*new Reservation_Seat
                {
                    ReservationId = 1,
                    SeatId = 5
                },
                new Reservation_Seat
                {
                    ReservationId = 2,
                    SeatId = 9
                },
                new Reservation_Seat
                {
                    ReservationId = 3,
                    SeatId = 1
                },
                new Reservation_Seat
                {
                    ReservationId = 4,
                    SeatId = 13
                },
                new Reservation_Seat
                {
                    ReservationId = 5,
                    SeatId = 8
                },
                new Reservation_Seat
                {
                    ReservationId = 6,
                    SeatId = 8
                },
                new Reservation_Seat
                {
                    ReservationId = 7,
                    SeatId = 12
                },
                new Reservation_Seat
                {
                    ReservationId = 8,
                    SeatId = 3
                }*/
            );
        }
    }
}
