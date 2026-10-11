using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Design;

namespace EventFlowAPI.DB.Context
{
    public class APIContextFactory : IDesignTimeDbContextFactory<APIContext>
    {
        public APIContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<APIContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-250K04R;Database=EventFlow;Trusted_Connection=True;TrustServerCertificate=True;")
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

            return new APIContext(optionsBuilder.Options);
        }
    }
}
