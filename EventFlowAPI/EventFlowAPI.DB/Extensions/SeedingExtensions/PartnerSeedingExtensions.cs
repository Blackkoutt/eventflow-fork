using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class PartnerSeedingExtensions
    {
        public static void Seed(this EntityTypeBuilder<Partner> entityBuilder)
        {
            entityBuilder.HasData(
                new Partner
                {
                    Id = 10,
                    Name = "Basel",
                    PartnerGuid = new Guid("8e0ab6f0-ebbd-4d96-ae19-574b276e985c"),
                    PhotoName = "basel.png"
                },
                new Partner
                {
                    Id = 11,
                    Name = "Aura",
                    PartnerGuid = new Guid("fa1fefc3-3a92-4162-a5cc-30aafea11323"),
                    PhotoName = "aura.png"
                },
                 new Partner
                 {
                     Id = 12,
                     Name = "Vision",
                     PartnerGuid = new Guid("bb243a20-bf87-4534-b9b2-a994004f00c4"),
                     PhotoName = "vision.png"
                 },
                 new Partner
                 {
                     Id = 13,
                     Name = "Snowflake",
                     PartnerGuid = new Guid("2a1dbe6b-42f3-4fec-ab63-dffba70184b7"),
                     PhotoName = "snowflake.png"
                 },
                 new Partner
                 {
                     Id = 14,
                     Name = "Waveless",
                     PartnerGuid = new Guid("20f68122-1bdf-4881-8ce4-f808701648a5"),
                     PhotoName = "waveless.png"
                 }
            );
        }
    }
}
