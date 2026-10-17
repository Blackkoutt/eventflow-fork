using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class PartnerSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<Partner> entityBuilder)
        {
            entityBuilder.HasData(
               new Partner
               {
                   Kind = PartnerKind.MediaPatron,
                   Id = 1,
                   Name = "Gazeta Nowoczesna", 
                   PartnerGuid = new Guid("ae371ef5-1408-4a99-9df5-c02da3894a64"),
                   PhotoName = "gazeta_nowoczesna.png"
               },
               new Partner
               {
                   Kind = PartnerKind.MediaPatron,
                   Id = 2,
                   Name = "Nowy świat TV",
                   PartnerGuid = new Guid("1888ac98-9879-49d8-b5a0-a9bdedf65b79"),
                   PhotoName = "nowy_swiat.png"
               },
               new Partner
               {
                   Kind = PartnerKind.MediaPatron,
                   Id = 3,
                   Name = "Tygodnik Nowiny",
                   PartnerGuid = new Guid("21721629-b1aa-4fd0-8fce-689d78d6c326"),
                   PhotoName = "tygodnik_nowiny.png"
               },
               new Partner
               {
                   Kind = PartnerKind.Organizer,
                   Id = 4,
                   Name = "EventFlow", 
                   PartnerGuid = new Guid("4656853b-fbf1-4407-8a15-9f60fe367343"),
                   PhotoName = "eventflow.png",
               },
               new Partner
               {
                   Kind = PartnerKind.Organizer,
                   Id = 5,
                   Name = "Snowflake",
                   PartnerGuid = new Guid("175c000d-26c4-4519-b49d-c9d1d7935699"),
                   PhotoName = "snowflake.png",
               },
               new Partner
               {
                   Kind = PartnerKind.Organizer,
                   Id = 6,
                   Name = "Aura",
                   PartnerGuid = new Guid("d044fdef-73a3-4f97-962f-d85e36760f4e"),
                   PhotoName = "aura.png",
               },
                new Partner
                {
                    Kind = PartnerKind.Sponsor,
                    Id = 7,
                    Name = "Basel",
                    PartnerGuid = new Guid("423d7369-b70c-4471-97a8-968d001718a7"),     
                    PhotoName = "basel.png"
                },
                new Partner
                {
                    Kind = PartnerKind.Sponsor,
                    Id = 8,
                    Name = "Vision",
                    PartnerGuid = new Guid("6ba10301-43f6-4207-835c-15f03d7904c7"),
                    PhotoName = "vision.png"
                },
                new Partner
                {
                    Kind = PartnerKind.Sponsor,
                    Id = 9,
                    Name = "Waveless",
                    PartnerGuid = new Guid("ca9f16d2-cfa6-47dc-9aff-a0821ce6bf79"),
                    PhotoName = "waveless.png"
                },
                new Partner
                {
                    Kind = PartnerKind.Partner,
                    Id = 10,
                    Name = "Basel",
                    PartnerGuid = new Guid("8e0ab6f0-ebbd-4d96-ae19-574b276e985c"),
                    PhotoName = "basel.png"
                },
                new Partner
                {
                    Kind = PartnerKind.Partner,
                    Id = 11,
                    Name = "Aura",
                    PartnerGuid = new Guid("fa1fefc3-3a92-4162-a5cc-30aafea11323"),
                    PhotoName = "aura.png"
                },
                 new Partner
                 {
                     Kind = PartnerKind.Partner,
                     Id = 12,
                     Name = "Vision",
                     PartnerGuid = new Guid("bb243a20-bf87-4534-b9b2-a994004f00c4"),
                     PhotoName = "vision.png"
                 },
                 new Partner
                 {
                     Kind = PartnerKind.Partner,
                     Id = 13,
                     Name = "Snowflake",
                     PartnerGuid = new Guid("2a1dbe6b-42f3-4fec-ab63-dffba70184b7"),
                     PhotoName = "snowflake.png"
                 },
                 new Partner
                 {
                     Kind = PartnerKind.Partner,
                     Id = 14,
                     Name = "Waveless",
                     PartnerGuid = new Guid("20f68122-1bdf-4881-8ce4-f808701648a5"),
                     PhotoName = "waveless.png"
                 }
            );
        }
    }
}
