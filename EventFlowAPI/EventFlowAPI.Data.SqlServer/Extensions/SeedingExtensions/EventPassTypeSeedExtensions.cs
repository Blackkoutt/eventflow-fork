using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class EventPassTypeSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<EventPassType> entityBuilder)
        {
            entityBuilder.HasData(
                new EventPassType
                {
                    Id = 1,
                    Name = "Karnet miesięczny",
                    ValidityPeriodInMonths = 1,
                    RenewalDiscountPercentage = 5m,
                    Price = 89.99m
                },
                new EventPassType
                {
                    Id = 2,
                    Name = "Karnet kwartalny",
                    ValidityPeriodInMonths = 3,
                    RenewalDiscountPercentage = 10m,
                    Price = 235.99m
                },
                new EventPassType
                {
                    Id = 3,
                    Name = "Karnet półroczny",
                    ValidityPeriodInMonths = 6,
                    RenewalDiscountPercentage = 15m,
                    Price = 499.99m
                },
                new EventPassType
                {
                    Id = 4,
                    Name = "Karnet roczny",
                    ValidityPeriodInMonths = 12,
                    RenewalDiscountPercentage = 20m,
                    Price = 999.99m
                }
            );
        }
    }
}
