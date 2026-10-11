using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class PaymentTypeSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<PaymentType> entityBuilder)
        {
            entityBuilder.HasData(
                new PaymentType
                {
                    Id = 1,
                    Name = "PayU",
                    PaymentTypeGuid = new Guid("2aa93a32-7232-48e7-a05b-8364c8aafffc"),
                },
                new PaymentType
                {
                    Id = 2,
                    Name = "Karnet",
                    PaymentTypeGuid = new Guid("1938093d-7427-40e5-a736-c2aa4305dc16"),
                }
            );
        }
    }
}
