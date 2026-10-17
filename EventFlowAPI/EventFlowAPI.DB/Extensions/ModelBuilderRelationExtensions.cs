using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.DB.Extensions
{
    public static class ModelBuilderRelationExtensions
    {
        public static void AddOneToOneRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UserData>().ToTable("Users");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<EventDetails>().ToTable("Event");
            modelBuilder.Entity<Festival>().ToTable("Festival");
            modelBuilder.Entity<FestivalDetails>().ToTable("Festival");
            modelBuilder.Entity<Hall>().ToTable("Hall");
            modelBuilder.Entity<HallDetails>().ToTable("Hall");

            modelBuilder.Entity<UserData>()
               .HasOne(x => x.User)
               .WithOne(x => x.UserData)
               .HasForeignKey<UserData>(x => x.Id);

            modelBuilder.Entity<HallDetails>()
               .HasOne(x => x.Hall)
               .WithOne(x => x.HallDetails)
               .HasForeignKey<HallDetails>(x => x.Id);

            modelBuilder.Entity<Hall>().Navigation(x => x.HallDetails).IsRequired();

            modelBuilder.Entity<EventDetails>()
                .HasOne(x => x.Event)
                .WithOne(x => x.Details)
                .HasForeignKey<EventDetails>(x => x.Id);

            modelBuilder.Entity<FestivalDetails>()
                .HasOne(x => x.Festival)
                .WithOne(x => x.Details)
                .HasForeignKey<FestivalDetails>(x => x.Id);
        }
        public static void AddManyToManyRelations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Festival>()
               .HasMany(e => e.Partners)
               .WithMany(e => e.Festivals)
               .UsingEntity<Festival_Partner>(
                    right => right.HasOne<Partner>().WithMany().HasForeignKey(x => x.PartnerId),
                    left => left.HasOne<Festival>().WithMany().HasForeignKey(x => x.FestivalId));

            modelBuilder.Entity<Festival>()
                .HasMany(e => e.Events)
                .WithMany(e => e.Festivals)
                .UsingEntity<Festival_Event>();

            modelBuilder.Entity<Reservation>()
                .HasMany(e => e.Seats)
                .WithMany(e => e.Reservations)
                .UsingEntity<Reservation_Seat>();

            modelBuilder.Entity<HallType>()
                .HasMany(e => e.Equipments)
                .WithMany(e => e.HallTypes)
                .UsingEntity<HallType_Equipment>();

            modelBuilder.Entity<HallRent>()
               .HasMany(e => e.AdditionalServices)
               .WithMany(e => e.Rents)
               .UsingEntity<HallRent_AdditionalServices>();
        }
    }
}
