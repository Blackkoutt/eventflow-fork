using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class MediaPatronSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<MediaPatron> entityBuilder)
        {
            entityBuilder.HasData(
               new MediaPatron
               {
                   Id = 1,
                   Name = "Gazeta Nowoczesna", 
                   MediaPatronGuid = new Guid("ae371ef5-1408-4a99-9df5-c02da3894a64"),
                   PhotoName = "gazeta_nowoczesna.png"
               },
               new MediaPatron
               {
                   Id = 2,
                   Name = "Nowy świat TV",
                   MediaPatronGuid = new Guid("1888ac98-9879-49d8-b5a0-a9bdedf65b79"),
                   PhotoName = "nowy_swiat.png"
               },
               new MediaPatron
               {
                   Id = 3,
                   Name = "Tygodnik Nowiny",
                   MediaPatronGuid = new Guid("21721629-b1aa-4fd0-8fce-689d78d6c326"),
                   PhotoName = "tygodnik_nowiny.png"
               }
            );
        }
    }
}
