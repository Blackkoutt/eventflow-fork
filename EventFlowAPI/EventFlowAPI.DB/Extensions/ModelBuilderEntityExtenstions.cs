using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.DB.Extensions
{
    public static class ModelBuilderEntityExtenstions
    {
        public static void AddEntitiesPrimaryKeys(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalServices>().HasKey(x => x.Id);
            modelBuilder.Entity<Equipment>().HasKey(x => x.Id);
            modelBuilder.Entity<Event>().HasKey(x => x.Id);
            modelBuilder.Entity<EventCategory>().HasKey(x => x.Id);
            modelBuilder.Entity<EventDetails>().HasKey(x => x.Id);
            modelBuilder.Entity<EventPass>().HasKey(x => x.Id);
            modelBuilder.Entity<EventPassType>().HasKey(x => x.Id);
            modelBuilder.Entity<Ticket>().HasKey(x => x.Id);
            modelBuilder.Entity<TicketType>().HasKey(x => x.Id);
            modelBuilder.Entity<Festival>().HasKey(x => x.Id);
            modelBuilder.Entity<Festival_Event>().HasKey(x => new { x.FestivalId, x.EventId });
            modelBuilder.Entity<Partner>().HasKey(x => x.Id);
            modelBuilder.Entity<Festival_Partner>().HasKey(x => new { x.FestivalId, x.PartnerId });
            modelBuilder.Entity<FestivalDetails>().HasKey(x => x.Id);
            modelBuilder.Entity<Hall>().HasKey(x => x.Id);
            modelBuilder.Entity<HallDetails>().HasKey(x => x.Id);
            modelBuilder.Entity<HallRent>().HasKey(x => x.Id);
            modelBuilder.Entity<HallRent_AdditionalServices>().HasKey(x => new { x.HallRentId, x.AdditionalServicesId });
            modelBuilder.Entity<HallType>().HasKey(x => x.Id);
           // modelBuilder.Entity<HallType_Equipment>().HasKey(x => new { x.EquipmentId, x.HallTypeId });
            modelBuilder.Entity<Reservation>().HasKey(x => x.Id);
            modelBuilder.Entity<Seat>().HasKey(x => x.Id);
            modelBuilder.Entity<SeatType>().HasKey(x => x.Id);
            modelBuilder.Entity<Reservation_Seat>().HasKey(x => new { x.ReservationId, x.SeatId });
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<UserData>().HasKey(x => x.Id);
        }
        public static void AddEntitiesConstraints(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().Property(x => x.Role).HasConversion<string>().HasMaxLength(20);

            modelBuilder.Entity<Partner>().ToTable("Partner");
            modelBuilder.Entity<Partner>().Property(x => x.Kind).HasConversion<string>().HasMaxLength(20);

            modelBuilder.HasSequence<int>("EventIdSequence").StartsAt(13);
            modelBuilder.Entity<Event>().Property(x => x.Id).UseSequence("EventIdSequence");

            modelBuilder.HasSequence<int>("FestivalIdSequence").StartsAt(4);
            modelBuilder.Entity<Festival>().Property(x => x.Id).UseSequence("FestivalIdSequence");

            
            /*modelBuilder.Entity<Hall>()
                .HasIndex(e => e.HallNr)
                .IsUnique();*/
        }
    }
}
