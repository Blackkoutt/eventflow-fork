using EventFlowAPI.DB.Extensions;
using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EventFlowAPI.DB.Context
{
    public class APIContext(DbContextOptions<APIContext> options) : IdentityDbContext<User, Role, string>(options)
    {
        public DbSet<AdditionalServices> AdditionalServices { get; set; }
        public DbSet<Equipment> Equipment{ get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<EventCategory> EventCategory { get; set; }
        public DbSet<EventDetails> EventDetails { get; set; }
        public DbSet<EventPass> EventPass { get; set; }
        public DbSet<EventPassType> EventPassType { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<Festival> Festival { get; set; }
        public DbSet<MediaPatron> MediaPatron { get; set; }
        public DbSet<Organizer> Organizer { get; set; }
        public DbSet<Sponsor> Sponsor { get; set; }
        public DbSet<Festival_Event> Festival_Event { get; set; }
        public DbSet<Festival_MediaPatron> Festival_MediaPatron { get; set; }
        public DbSet<Festival_Organizer> Festival_Organizer { get; set; }
        public DbSet<Festival_Sponsor> Festival_Sponsor { get; set; }
        public DbSet<FestivalDetails> FestivalDetails { get; set; }
        public DbSet<Hall> Hall { get; set; }
        public DbSet<HallDetails> HallDetails { get; set; }
        public DbSet<HallRent> HallRent { get; set; }
        public DbSet<HallRent_AdditionalServices> HallRent_AdditionalServices { get; set; }
        public DbSet<HallType> HallType { get; set; }
        public DbSet<HallType_Equipment> HallType_Equipment { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Seat> Seat { get; set; }
        public DbSet<TicketJPG> TicketJPG { get; set; }
        public DbSet<TicketPDF> TicketPDF { get; set; }
        public DbSet<SeatType> SeatType { get; set; }
        public DbSet<Reservation_Seat> Reservation_Seat { get; set; }
        public DbSet<Reservation_TicketJPG> Reservation_TicketJPG { get; set; }
        public DbSet<UserData> UserData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Primary keys
            modelBuilder.AddEntitiesPrimaryKeys();

            // Relations 1:1
            modelBuilder.AddOneToOneRelations();

            // Constraints
            modelBuilder.AddEntitiesConstraints();

            // Relations N:N
            modelBuilder.AddManyToManyRelations();

            // Seed the data
            modelBuilder.Seed();
        }
    }
}
