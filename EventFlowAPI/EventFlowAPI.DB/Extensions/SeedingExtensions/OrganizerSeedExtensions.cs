using EventFlowAPI.DB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventFlowAPI.DB.Extensions.SeedingExtensions
{
    public static class OrganizerSeedExtensions
    {
        public static void Seed(this EntityTypeBuilder<Organizer> entityBuilder)
        {
            entityBuilder.HasData(
               new Organizer
               {
                   Id = 1,
                   Name = "EventFlow", 
                   OrganizerGuid = new Guid("4656853b-fbf1-4407-8a15-9f60fe367343"),
                   PhotoName = "eventflow.png",
               },
               new Organizer
               {
                   Id = 2,
                   Name = "Snowflake",
                   OrganizerGuid = new Guid("175c000d-26c4-4519-b49d-c9d1d7935699"),
                   PhotoName = "snowflake.png",
               },
               new Organizer
               {
                   Id = 3,
                   Name = "Aura",
                   OrganizerGuid = new Guid("d044fdef-73a3-4f97-962f-d85e36760f4e"),
                   PhotoName = "aura.png",
               }
            );
        }
    }
}
