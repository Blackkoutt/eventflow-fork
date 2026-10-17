using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.DB.Extensions
{
    public static class ModelBuilderRelationExtensions
    {
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
