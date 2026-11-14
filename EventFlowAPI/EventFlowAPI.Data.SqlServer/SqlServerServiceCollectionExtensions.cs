using EventFlowAPI.Data.SqlServer.Context;
using EventFlowAPI.Logic.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlowAPI.Data.SqlServer
{
    public static class SqlServerServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the relational implementation of the data layer (SQL Server, EF Core).
        /// </summary>
        public static IServiceCollection AddSqlServerDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<APIContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
