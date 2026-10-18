using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class EventPassSeedExtentions
    {
        public static void Seed(this EntityTypeBuilder<EventPass> entityBuilder, DateTime today)
        {

            var epGuid1 = new Guid("b00ca94a-e6b2-4d2e-b270-244b3e76048d");
            var epGuid2 = new Guid("766245b4-8c08-49dd-9480-2606aaa590be");
            var epGuid3 = new Guid("33610a0d-a1b7-4700-bffe-9e334b977e6a");

            entityBuilder.HasData(
                new EventPass
                {
                    Id = 1,
                    EventPassGuid = epGuid1,
                    StartDate = today,
                    RenewalDate = null,
                    EndDate = today.AddMonths(6),
                    PaymentDate = today,
                    PaymentAmount = 499.99m,
                    PassTypeId = 3,
                    TotalDiscount = 0m,
                    TotalDiscountPercentage = 0m,
                    UserId = "2",
                    PaymentType = PaymentType.PayU
                },

                new EventPass
                {
                    Id = 2,
                    EventPassGuid = epGuid2,
                    StartDate = today,
                    RenewalDate = null,
                    EndDate = today.AddYears(1),
                    PaymentDate = today,
                    PaymentAmount = 999.99m,
                    PassTypeId = 4,
                    TotalDiscount = 0m,
                    TotalDiscountPercentage = 0m,
                    UserId = "3",
                    PaymentType = PaymentType.PayU
                },
                new EventPass
                {
                    Id = 3,
                    EventPassGuid = epGuid3,
                    StartDate = today,
                    RenewalDate = null,
                    EndDate = today.AddMonths(3),
                    PaymentDate = today,
                    PaymentAmount = 235.99m,
                    PassTypeId = 2,
                    TotalDiscount = 0m,
                    TotalDiscountPercentage = 0m,
                    UserId = "4",
                    PaymentType = PaymentType.PayU
                }
            );
        }
    }
}
