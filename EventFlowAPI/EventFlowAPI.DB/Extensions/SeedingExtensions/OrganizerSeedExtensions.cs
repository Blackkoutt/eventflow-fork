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
                   Id = 4,
                   Name = "EventFlow", 
                   PartnerGuid = new Guid("4656853b-fbf1-4407-8a15-9f60fe367343"),
                   PhotoName = "eventflow.png",
               },
               new Organizer
               {
                   Id = 5,
                   Name = "Snowflake",
                   PartnerGuid = new Guid("175c000d-26c4-4519-b49d-c9d1d7935699"),
                   PhotoName = "snowflake.png",
               },
               new Organizer
               {
                   Id = 6,
                   Name = "Aura",
                   PartnerGuid = new Guid("d044fdef-73a3-4f97-962f-d85e36760f4e"),
                   PhotoName = "aura.png",
               }
            );
        }
    }
}
