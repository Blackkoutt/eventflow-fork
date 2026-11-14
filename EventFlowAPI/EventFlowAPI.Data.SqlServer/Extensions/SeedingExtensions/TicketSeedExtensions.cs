using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.Data.SqlServer.Extensions.SeedingExtensions
{
    public static class TicketSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<Ticket> entityBuilder)
        {
            entityBuilder.HasData(
                new Ticket
                {
                    Id = 1,
                    Price = 24.99m,
                    EventId = 1,
                    TicketTypeId = 1
                },
                new Ticket
                {
                    Id = 2,
                    Price = 34.99m,
                    EventId = 2,
                    TicketTypeId = 1
                },
                new Ticket
                {
                    Id = 3,
                    Price = 29.99m,
                    EventId = 3,
                    TicketTypeId = 1
                },
                new Ticket
                {
                    Id = 4,
                    Price = 19.99m,
                    EventId = 4,
                    TicketTypeId = 1
                },
                new Ticket
                {
                    Id = 5,
                    Price = 19.99m,
                    EventId = 1,
                    FestivalId = 1,
                    TicketTypeId = 1
                },
                new Ticket
                {
                    Id = 6,
                    Price = 19.99m,
                    EventId = 5,
                    FestivalId = 1,
                    TicketTypeId = 1
                },
                new Ticket
                {
                    Id = 7,
                    Price = 29.99m,
                    EventId = 2,
                    FestivalId = 2,
                    TicketTypeId = 2
                },
                new Ticket
                {
                    Id = 8,
                    Price = 29.99m,
                    EventId = 6,
                    FestivalId = 2,
                    TicketTypeId = 2
                },
                 new Ticket
                 {
                     Id = 9,
                     Price = 24.99m,
                     EventId = 5,
                     TicketTypeId = 1
                 },
                new Ticket
                {
                    Id = 10,
                    Price = 32.99m,
                    EventId = 6,
                    TicketTypeId = 1
                },
                new Ticket
                {
                    Id = 11,
                    Price = 34.99m,
                    EventId = 7,
                    TicketTypeId = 1
                },
                new Ticket
                {
                    Id = 12,
                    Price = 19.99m,
                    EventId = 8,
                    TicketTypeId = 1
                }
            );
        }
    }
}
