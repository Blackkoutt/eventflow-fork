using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class ReservationSeedExtentions
    {
        public static void Seed(this EntityTypeBuilder<Reservation> entityBuilder, DateTime today)
        {
            var res1 = new Guid("53538b58-f885-4f4a-b675-a4aa4063ccf3");
            var res2 = new Guid("ed8b9230-223b-4609-8d13-aa6017edad09");
            var res3 = new Guid("f9a076c4-3475-4a28-a60c-6e0e3c03731a");
            var res4 = new Guid("de1d6773-f027-4888-996a-0296e5c52708");
            var firstFestivalGuid = new Guid("0b74ec7b-933b-4163-afa5-e0997681dccd");
            var secondFestivalGuid = new Guid("806cade1-2685-43dc-8cfc-682fc4229db6");

            entityBuilder.HasData(
                new Reservation
                {
                    Id = 1,
                    ReservationGuid = res1,
                    ReservationDate = today,
                    StartDate = today.AddMonths(1).AddDays(1),
                    EndDate = today.AddMonths(1).AddDays(1).AddHours(1),
                    TotalAddtionalPaymentPercentage = 10m,
                    TotalAdditionalPaymentAmount = 2.5m,
                    PaymentAmount = 24.99m,
                    UserId = "4",
                    PaymentType = PaymentType.PayU,
                    TicketId = 1,
                    TicketPDFFileName = $"eventflow_bilet_test_{res1}.pdf",
                    TicketImagesFolder = $"{res1}"
                },
                new Reservation
                {
                    Id = 2,
                    ReservationGuid = res2,
                    ReservationDate = today,
                    StartDate = today.AddMonths(1).AddDays(2),
                    EndDate = today.AddMonths(1).AddDays(2).AddHours(3),
                    TotalAddtionalPaymentPercentage = 0m,
                    TotalAdditionalPaymentAmount = 0m,
                    PaymentAmount = 34.99m,
                    UserId = "2",
                    PaymentType = PaymentType.PayU,
                    TicketId = 2,
                    TicketPDFFileName = $"eventflow_bilet_test_{res2}.pdf",
                    TicketImagesFolder = $"{res2}"
                },
                new Reservation
                {
                    Id = 3,
                    ReservationGuid = res3,
                    ReservationDate = today,
                    StartDate = today.AddMonths(1).AddDays(3),
                    EndDate = today.AddMonths(1).AddDays(3).AddHours(2),
                    TotalAddtionalPaymentPercentage = 25m,
                    TotalAdditionalPaymentAmount = 7.5m,
                    PaymentAmount = 29.99m,
                    UserId = "3",
                    PaymentType = PaymentType.PayU,
                    TicketId = 3,
                    TicketPDFFileName = $"eventflow_bilet_test_{res3}.pdf",
                    TicketImagesFolder = $"{res3}"
                },
                new Reservation
                {
                    Id = 4,
                    ReservationGuid = res4,
                    ReservationDate = today,
                    StartDate = today.AddMonths(1).AddDays(4),
                    EndDate = today.AddMonths(1).AddDays(4).AddHours(3),
                    TotalAddtionalPaymentPercentage = 0m,
                    TotalAdditionalPaymentAmount = 0m,
                    PaymentAmount = 19.99m,
                    UserId = "3",
                    PaymentType = PaymentType.PayU,
                    TicketId = 4,
                    TicketPDFFileName = $"eventflow_bilet_test_{res4}.pdf",
                    TicketImagesFolder = $"{res4}"
                },
                new Reservation
                {
                    Id = 5,
                    ReservationGuid = firstFestivalGuid,
                    ReservationDate = today,
                    StartDate = today.AddMonths(1).AddDays(1),
                    EndDate = today.AddMonths(1).AddDays(1).AddHours(1),
                    TotalAddtionalPaymentPercentage = 10m,
                    TotalAdditionalPaymentAmount = 2m,
                    PaymentAmount = 19.99m,
                    UserId = "2",
                    PaymentType = PaymentType.PayU,
                    TicketId = 5,
                    TicketPDFFileName = $"eventflow_bilet_test_{firstFestivalGuid}.pdf",
                    TicketImagesFolder = $"{firstFestivalGuid}"
                },
                new Reservation
                {
                    Id = 6,
                    ReservationGuid = firstFestivalGuid,
                    ReservationDate = today,
                    StartDate = today.AddMonths(2).AddDays(1),
                    EndDate = today.AddMonths(2).AddDays(1).AddHours(1),
                    TotalAddtionalPaymentPercentage = 10m,
                    TotalAdditionalPaymentAmount = 2m,
                    PaymentAmount = 19.99m,
                    UserId = "2",
                    PaymentType = PaymentType.PayU,
                    TicketId = 6,
                    TicketPDFFileName = $"eventflow_bilet_test_{firstFestivalGuid}.pdf",
                    TicketImagesFolder = $"{firstFestivalGuid}"
                },
                new Reservation
                {
                    Id = 7,
                    ReservationGuid = secondFestivalGuid,
                    ReservationDate = today,
                    StartDate = today.AddMonths(1).AddDays(2),
                    EndDate = today.AddMonths(1).AddDays(2).AddHours(3),
                    TotalAddtionalPaymentPercentage = 0m,
                    TotalAdditionalPaymentAmount = 0m,
                    PaymentAmount = 29.99m,
                    UserId = "2",
                    PaymentType = PaymentType.PayU,
                    TicketId = 7,
                    TicketPDFFileName = $"eventflow_bilet_test_{secondFestivalGuid}.pdf",
                    TicketImagesFolder = $"{secondFestivalGuid}"
                },
                new Reservation
                {
                    Id = 8,
                    ReservationGuid = secondFestivalGuid,
                    ReservationDate = today,
                    StartDate = today.AddMonths(2).AddDays(3),
                    EndDate = today.AddMonths(2).AddDays(3).AddHours(2),
                    TotalAddtionalPaymentPercentage = 0m,
                    TotalAdditionalPaymentAmount = 0m,
                    PaymentAmount = 29.99m,
                    UserId = "2",
                    PaymentType = PaymentType.PayU,
                    TicketId = 8,
                    TicketPDFFileName = $"eventflow_bilet_test_{secondFestivalGuid}.pdf",
                    TicketImagesFolder = $"{secondFestivalGuid}"
                }
            );
        }
    }
}
