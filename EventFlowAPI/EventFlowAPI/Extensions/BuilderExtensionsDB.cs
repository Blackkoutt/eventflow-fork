using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.DB.Context;
using EventFlowAPI.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventFlowAPI.Extensions
{
    public static class BuilderExtensionsDB
    {
        public static void AddConnectionToDB(this WebApplicationBuilder builder, ConnectionString connectionString)
        {
            builder.Services.AddDbContext<APIContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString.ToString()));
            });
        }
        public static void AddLocalFileStorage(this WebApplicationBuilder builder)
        {
            var configuredPath = builder.Configuration.GetSection("LocalStorage")["RootPath"] ?? "Storage";
            var rootPath = Path.IsPathRooted(configuredPath)
                ? configuredPath
                : Path.Combine(builder.Environment.ContentRootPath, configuredPath);

            Directory.CreateDirectory(rootPath);
            builder.Services.AddSingleton(new LocalStorageSettings(rootPath));
        }
    }
}
