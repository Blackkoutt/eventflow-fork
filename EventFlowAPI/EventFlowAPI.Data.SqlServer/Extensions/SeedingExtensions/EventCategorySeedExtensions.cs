using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class EventCategorySeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<EventCategory> entityBuilder)
        {
            entityBuilder.HasData(
                new EventCategory
                {
                    Id = 1,
                    Name = "Koncert",
                    Icon = "fa-solid fa-music",
                    Color = "#82CAFC"
                },
                new EventCategory
                {
                    Id = 2,
                    Name = "Film",
                    Icon = "fa-solid fa-film",
                    Color = "#6BD49B"
                },
                new EventCategory
                {
                    Id = 3,
                    Name = "Spektakl",
                    Icon = "fa-solid fa-masks-theater",
                    Color = "#C33EB1"
                },
                new EventCategory
                {
                    Id = 4,
                    Name = "Wystawa",
                    Icon = "fa-solid fa-landmark",
                    Color = "#FC5353"
                }
            );
        }
    }
}
