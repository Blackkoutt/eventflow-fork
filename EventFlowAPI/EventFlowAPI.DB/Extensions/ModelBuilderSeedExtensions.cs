using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Extensions.SeedingExtensions;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.DB.Extensions
{
    public static class ModelBuilderSeedExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var today = new DateTime(2025, 03, 18);


            // Seed Users
            modelBuilder.SeedUsersData();
            modelBuilder.SeedUsers(today);

            // Seed Entities
            modelBuilder.Entity<FAQ>().Seed();
            modelBuilder.Entity<News>().Seed(today);
            modelBuilder.Entity<Partner>().Seed();
            modelBuilder.Entity<AdditionalServices>().Seed();            
            modelBuilder.Entity<Equipment>().Seed();
            modelBuilder.Entity<EventCategory>().Seed();
            modelBuilder.Entity<EventDetails>().Seed();
            modelBuilder.Entity<EventPassType>().Seed();
            modelBuilder.Entity<FestivalDetails>().Seed();
            modelBuilder.Entity<HallType>().Seed();
            modelBuilder.Entity<SeatType>().Seed();
            modelBuilder.Entity<TicketType>().Seed();
            modelBuilder.Entity<Festival>().Seed(today: today);
            modelBuilder.Entity<HallDetails>().Seed();
            modelBuilder.Entity<Hall>().Seed();
            modelBuilder.Entity<HallType_Equipment>().Seed();
            modelBuilder.Entity<Festival_Partner>().Seed();
            modelBuilder.Entity<Event>().Seed(today: today);
            modelBuilder.Entity<Seat>().Seed();
            modelBuilder.Entity<EventPass>().Seed(today: today);
            modelBuilder.Entity<HallRent>().Seed(today: today);
            modelBuilder.Entity<Ticket>().Seed();
            modelBuilder.Entity<Festival_Event>().Seed();
            modelBuilder.Entity<HallRent_AdditionalServices>().Seed();
            modelBuilder.Entity<Reservation>().Seed(today: today);
            modelBuilder.Entity<Reservation_Seat>().Seed();
        }
    }
}
