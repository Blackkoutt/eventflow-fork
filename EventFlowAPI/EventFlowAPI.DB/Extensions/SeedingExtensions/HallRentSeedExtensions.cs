using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class HallRentSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<HallRent> entityBuilder, DateTime today)
        {
            var hrGuid1 = new Guid("48972d7a-ab0d-4827-bdd6-2002dc1f1e08");
            var hrGuid2 = new Guid("3ad58762-60f0-4753-bdf7-c9c7e3eb775c");
            var hrGuid3 = new Guid("38daa558-0f6e-414a-913c-c500d41d5dc5");
            var hrGuid4 = new Guid("e690b490-7387-490c-89df-bd9eaf2cc36b");

            entityBuilder.HasData(
                new HallRent
                {
                    Id = 1,
                    HallRentGuid = hrGuid1,
                    StartDate = today.AddMonths(1).AddDays(1),
                    EndDate = today.AddMonths(1).AddDays(1).AddHours(8),
                    RentDate = today,
                    PaymentAmount = 899.99m,
                    PaymentType = PaymentType.PayU,
                    HallId = 12,
                    UserId = "4"
                },
                new HallRent
                {
                    Id = 2,
                    HallRentGuid = hrGuid2,
                    StartDate = today.AddMonths(1).AddDays(2),
                    EndDate = today.AddMonths(1).AddDays(2).AddHours(4),
                    RentDate = today,
                    PaymentAmount = 699.99m,
                    PaymentType = PaymentType.PayU,
                    HallId = 13,
                    UserId = "3"
                },
                new HallRent
                {
                    Id = 3,
                    HallRentGuid = hrGuid3,
                    StartDate = today.AddMonths(1).AddDays(3),
                    EndDate = today.AddMonths(1).AddDays(3).AddHours(2),
                    RentDate = today,
                    PaymentAmount = 399.99m,
                    PaymentType = PaymentType.PayU,
                    HallId = 14,
                    UserId = "3"
                },
                new HallRent
                {
                    Id = 4,
                    HallRentGuid = hrGuid4,
                    StartDate = today.AddMonths(1).AddDays(4),
                    EndDate = today.AddMonths(1).AddDays(4).AddHours(1),
                    RentDate = today,
                    PaymentAmount = 150.99m,
                    PaymentType = PaymentType.PayU,
                    HallId = 15,
                    UserId = "2"
                }
            );
        }
    }
}
