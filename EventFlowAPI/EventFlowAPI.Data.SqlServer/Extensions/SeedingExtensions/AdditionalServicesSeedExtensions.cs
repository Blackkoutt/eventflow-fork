using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class AdditionalServicesSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<AdditionalServices> entityBuilder)
        {
            entityBuilder.HasData(
                new AdditionalServices
                {
                    Id = 1,
                    Name = "DJ",
                    Price = 400.00m
                },
                new AdditionalServices
                {
                    Id = 2,
                    Name = "Obsługa oświetlenia",
                    Price = 340.00m
                },
                new AdditionalServices
                {
                    Id = 3,
                    Name = "Obsługa nagłośnienia",
                    Price = 250.00m
                },
                new AdditionalServices
                {
                    Id = 4,
                    Name = "Fotograf",
                    Price = 200.00m
                },
                new AdditionalServices
                {
                    Id = 5,
                    Name = "Promocja wydarzenia",
                    Price = 140.00m
                },
                new AdditionalServices
                {
                    Id = 6,
                    Name = "Kamerzysta",
                    Price = 500.00m
                },
                new AdditionalServices
                {
                    Id = 7,
                    Name = "Catering",
                    Price = 1000.00m
                }
            );
        }
    }
}
