using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class SeatTypeSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<SeatType> entityBuilder)
        {
            entityBuilder.HasData(
                new SeatType
                {
                    Id = 1,
                    Name = "Miejsce VIP",
                    Description = "Opis miejsca VIP",
                    SeatColor = "#9803fc",
                    AddtionalPaymentPercentage = 25.00m
                },
                new SeatType
                {
                    Id = 2,
                    Name = "Miejsce klasy premium",
                    SeatColor = "#ffa600",
                    Description = "Opis miejsca klasy premium",
                    AddtionalPaymentPercentage = 10.00m
                },
                new SeatType
                {
                    Id = 3,
                    Name = "Miejsce zwykłe",
                    SeatColor = "#039aff",
                    Description = "Opis miejsca zwykłego",
                    AddtionalPaymentPercentage = 0.00m
                }
            );
        }
    }
}
