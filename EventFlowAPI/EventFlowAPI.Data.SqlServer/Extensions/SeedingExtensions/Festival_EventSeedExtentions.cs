using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class Festival_EventSeedExtentions
    {
        public static void Seed(this EntityTypeBuilder<Festival_Event> entityBuilder)
        {
            entityBuilder.HasData(
                new Festival_Event
                {
                    FestivalId = 1,
                    EventId = 1
                },
                new Festival_Event
                {
                    FestivalId = 1,
                    EventId = 5
                },
                new Festival_Event
                {
                    FestivalId = 2,
                    EventId = 2
                },
                new Festival_Event
                {
                    FestivalId = 2,
                    EventId = 6
                },
                new Festival_Event
                {
                    FestivalId = 3,
                    EventId = 4
                },
                new Festival_Event
                {
                    FestivalId = 3,
                    EventId = 7
                }
            );
        }
    }
}
