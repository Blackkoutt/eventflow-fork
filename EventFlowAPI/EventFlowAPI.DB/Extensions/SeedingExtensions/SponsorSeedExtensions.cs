using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class SponsorSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<Sponsor> entityBuilder)
        {
            entityBuilder.HasData(
                new Sponsor
                {
                    Id = 7,
                    Name = "Basel",
                    PartnerGuid = new Guid("423d7369-b70c-4471-97a8-968d001718a7"),     
                    PhotoName = "basel.png"
                },
                new Sponsor
                {
                    Id = 8,
                    Name = "Vision",
                    PartnerGuid = new Guid("6ba10301-43f6-4207-835c-15f03d7904c7"),
                    PhotoName = "vision.png"
                },
                new Sponsor
                {
                    Id = 9,
                    Name = "Waveless",
                    PartnerGuid = new Guid("ca9f16d2-cfa6-47dc-9aff-a0821ce6bf79"),
                    PhotoName = "waveless.png"
                }
            );
        }
    }
}
