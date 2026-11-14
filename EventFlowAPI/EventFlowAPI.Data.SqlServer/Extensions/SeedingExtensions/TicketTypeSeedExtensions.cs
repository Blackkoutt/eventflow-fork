using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class TicketTypeSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<TicketType> entityBuilder)
        {
            entityBuilder.HasData(
                new TicketType
                {
                    Id = 1,
                    Name = "Bilet normalny",

                },
                new TicketType
                {
                    Id = 2,
                    Name = "Bilet ulgowy",
                },
                new TicketType
                {
                    Id = 3,
                    Name = "Bilet rodzinny",
                }
            );
        }
    }
}
