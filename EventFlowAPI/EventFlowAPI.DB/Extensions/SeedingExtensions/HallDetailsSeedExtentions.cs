using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class HallDetailsSeedExtentions
    {
        public static void Seed(this EntityTypeBuilder<HallDetails> entityBuilder)
        {
            entityBuilder.HasData(
               new HallDetails
               {
                   Id = 1,
                   TotalLength = 22m,
                   TotalWidth = 14m, 
                   StageWidth = 7f,
                   StageLength = 4.4f,
                   MaxNumberOfSeatsRows = 15,
                   MaxNumberOfSeatsColumns = 15,
                   NumberOfSeats = 100,
                   MaxNumberOfSeats = 255,
               },
               new HallDetails
               {
                   Id = 2,
                   TotalLength = 22m,
                   TotalWidth = 9m,
                   StageWidth = null,
                   StageLength = null,
                   MaxNumberOfSeatsRows = 15,
                   MaxNumberOfSeatsColumns = 10,
                   NumberOfSeats = 150,
                   MaxNumberOfSeats = 150,
               },
               new HallDetails
               {
                   Id = 3,
                   TotalLength = 11m,
                   TotalWidth = 9m,
                   StageWidth = 5.6f,
                   StageLength = 4.4f,
                   MaxNumberOfSeatsRows = 6,
                   MaxNumberOfSeatsColumns = 10,
                   NumberOfSeats = 60,
                   MaxNumberOfSeats = 60,
               },
               new HallDetails
               {
                   Id = 4,
                   TotalLength = 17m,
                   TotalWidth = 9m,
                   StageWidth = 7.2f,
                   StageLength = 6f,
                   MaxNumberOfSeatsRows = 10,
                   MaxNumberOfSeatsColumns = 10,
                   NumberOfSeats = 100,
                   MaxNumberOfSeats = 100,
               },
                new HallDetails
                {
                    Id = 5,
                    TotalLength = 22m,
                    TotalWidth = 14m,
                    StageWidth = 7f,
                    StageLength = 4.4f,
                    MaxNumberOfSeatsRows = 15,
                    MaxNumberOfSeatsColumns = 15,
                    NumberOfSeats = 100,
                    MaxNumberOfSeats = 255,
                },
                new HallDetails
                {
                    Id = 6,
                    TotalLength = 22m,
                    TotalWidth = 9m,
                    StageWidth = null,
                    StageLength = null,
                    MaxNumberOfSeatsRows = 15,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 150,
                    MaxNumberOfSeats = 150,
                },
                new HallDetails
                {
                    Id = 7,
                    TotalLength = 11m,
                    TotalWidth = 9m,
                    StageWidth = 5.6f,
                    StageLength = 4.4f,
                    MaxNumberOfSeatsRows = 6,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 60,
                    MaxNumberOfSeats = 60,
                },
                new HallDetails
                {
                    Id = 8,
                    TotalLength = 17m,
                    TotalWidth = 9m,
                    StageWidth = 7.2f,
                    StageLength = 6f,
                    MaxNumberOfSeatsRows = 10,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 100,
                    MaxNumberOfSeats = 100,
                },
                new HallDetails
                {
                    Id = 9,
                    TotalLength = 22m,
                    TotalWidth = 14m,
                    StageWidth = 7f,
                    StageLength = 4.4f,
                    MaxNumberOfSeatsRows = 15,
                    MaxNumberOfSeatsColumns = 15,
                    NumberOfSeats = 100,
                    MaxNumberOfSeats = 255,
                },
                new HallDetails
                {
                    Id = 10,
                    TotalLength = 22m,
                    TotalWidth = 9m,
                    StageWidth = null,
                    StageLength = null,
                    MaxNumberOfSeatsRows = 15,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 150,
                    MaxNumberOfSeats = 150,
                },
                new HallDetails
                {
                    Id = 11,
                    TotalLength = 17m,
                    TotalWidth = 9m,
                    StageWidth = 7.2f,
                    StageLength = 6f,
                    MaxNumberOfSeatsRows = 10,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 100,
                    MaxNumberOfSeats = 100,
                },
                new HallDetails
                {
                    Id = 12,
                    TotalLength = 22m,
                    TotalWidth = 14m,
                    StageWidth = 7f,
                    StageLength = 4.4f,
                    MaxNumberOfSeatsRows = 15,
                    MaxNumberOfSeatsColumns = 15,
                    NumberOfSeats = 100,
                    MaxNumberOfSeats = 255,
                },
                new HallDetails
                {
                    Id = 13,
                    TotalLength = 22m,
                    TotalWidth = 9m,
                    StageWidth = null,
                    StageLength = null,
                    MaxNumberOfSeatsRows = 15,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 150,
                    MaxNumberOfSeats = 150,
                },
                new HallDetails
                {
                    Id = 14,
                    TotalLength = 11m,
                    TotalWidth = 9m,
                    StageWidth = 5.6f,
                    StageLength = 4.4f,
                    MaxNumberOfSeatsRows = 6,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 60,
                    MaxNumberOfSeats = 60,
                },
                new HallDetails
                {
                    Id = 15,
                    TotalLength = 17m,
                    TotalWidth = 9m,
                    StageWidth = 7.2f,
                    StageLength = 6f,
                    MaxNumberOfSeatsRows = 10,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 100,
                    MaxNumberOfSeats = 100,
                },
                new HallDetails
                {
                    Id = 16,
                    TotalLength = 11m,
                    TotalWidth = 9m,
                    StageWidth = 5.6f,
                    StageLength = 4.4f,
                    MaxNumberOfSeatsRows = 6,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 60,
                    MaxNumberOfSeats = 60,
                },
                new HallDetails
                {
                    Id = 17,
                    TotalLength = 22m,
                    TotalWidth = 9m,
                    StageWidth = null,
                    StageLength = null,
                    MaxNumberOfSeatsRows = 15,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 150,
                    MaxNumberOfSeats = 150,
                },
                new HallDetails
                {
                    Id = 18,
                    TotalLength = 22m,
                    TotalWidth = 14m,
                    StageWidth = 7f,
                    StageLength = 4.4f,
                    MaxNumberOfSeatsRows = 15,
                    MaxNumberOfSeatsColumns = 15,
                    NumberOfSeats = 100,
                    MaxNumberOfSeats = 255,
                },
                new HallDetails
                {
                    Id = 19,
                    TotalLength = 11m,
                    TotalWidth = 9m,
                    StageWidth = 5.6f,
                    StageLength = 4.4f,
                    MaxNumberOfSeatsRows = 6,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 60,
                    MaxNumberOfSeats = 60,
                },
                new HallDetails
                {
                    Id = 20,
                    TotalLength = 17m,
                    TotalWidth = 9m,
                    StageWidth = 7.2f,
                    StageLength = 6f,
                    MaxNumberOfSeatsRows = 10,
                    MaxNumberOfSeatsColumns = 10,
                    NumberOfSeats = 100,
                    MaxNumberOfSeats = 100,
                }
           );
        }
    }
}
