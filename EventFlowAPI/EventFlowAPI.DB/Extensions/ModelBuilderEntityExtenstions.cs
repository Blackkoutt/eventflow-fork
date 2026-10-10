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
            modelBuilder.Entity<MediaPatron>().HasKey(x => x.Id);
            modelBuilder.Entity<Organizer>().HasKey(x => x.Id);
            modelBuilder.Entity<Sponsor>().HasKey(x => x.Id);
            modelBuilder.Entity<Festival_Event>().HasKey(x => new { x.FestivalId, x.EventId });
            modelBuilder.Entity<Festival_MediaPatron>().HasKey(x => new { x.FestivalId, x.MediaPatronId });
            modelBuilder.Entity<Festival_Organizer>().HasKey(x => new { x.FestivalId, x.OrganizerId });
            modelBuilder.Entity<Festival_Sponsor>().HasKey(x => new { x.FestivalId, x.SponsorId });
            modelBuilder.Entity<FestivalDetails>().HasKey(x => x.Id);
            modelBuilder.Entity<Hall>().HasKey(x => x.Id);
            modelBuilder.Entity<HallDetails>().HasKey(x => x.Id);
            modelBuilder.Entity<HallRent>().HasKey(x => x.Id);
            modelBuilder.Entity<HallRent_AdditionalServices>().HasKey(x => new { x.HallRentId, x.AdditionalServicesId });
            modelBuilder.Entity<HallType>().HasKey(x => x.Id);
           // modelBuilder.Entity<HallType_Equipment>().HasKey(x => new { x.EquipmentId, x.HallTypeId });
            modelBuilder.Entity<PaymentType>().HasKey(x => x.Id);
            modelBuilder.Entity<Reservation>().HasKey(x => x.Id);
            modelBuilder.Entity<Seat>().HasKey(x => x.Id);
            modelBuilder.Entity<SeatType>().HasKey(x => x.Id);
            modelBuilder.Entity<TicketJPG>().HasKey(x => x.Id); 
            modelBuilder.Entity<TicketPDF>().HasKey(x => x.Id);
            modelBuilder.Entity<Reservation_Seat>().HasKey(x => new { x.ReservationId, x.SeatId });
            modelBuilder.Entity<Reservation_TicketJPG>().HasKey(x => new { x.ReservationId, x.TicketJPGId });
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<UserData>().HasKey(x => x.Id);
        }
        public static void AddEntitiesConstraints(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();

            
            /*modelBuilder.Entity<Hall>()
                .HasIndex(e => e.HallNr)
                .IsUnique();*/
        }
    }
}
