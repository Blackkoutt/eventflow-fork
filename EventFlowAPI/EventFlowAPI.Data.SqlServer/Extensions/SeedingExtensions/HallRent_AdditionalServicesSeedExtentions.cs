using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class HallRent_AdditionalServicesSeedExtentions
    {
        public static void Seed(this EntityTypeBuilder<HallRent_AdditionalServices> entityBuilder)
        {
            entityBuilder.HasData(
                new HallRent_AdditionalServices
                {
                    HallRentId = 2,
                    AdditionalServicesId = 1
                },
                new HallRent_AdditionalServices
                {
                    HallRentId = 2,
                    AdditionalServicesId = 2
                }, new HallRent_AdditionalServices
                {
                    HallRentId = 2,
                    AdditionalServicesId = 3
                },
                new HallRent_AdditionalServices
                {
                    HallRentId = 3,
                    AdditionalServicesId = 2
                },
                new HallRent_AdditionalServices
                {
                    HallRentId = 4,
                    AdditionalServicesId = 4
                },
                new HallRent_AdditionalServices
                {
                    HallRentId = 4,
                    AdditionalServicesId = 5
                }
            );
        }
    }
}
