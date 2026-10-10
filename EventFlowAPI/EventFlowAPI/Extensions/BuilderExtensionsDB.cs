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
            var contentRootPath = builder.Environment.ContentRootPath;
            var staticPath = builder.Configuration["LocalStorage:StaticPath"]
                ?? throw new InvalidOperationException("Missing configuration value 'LocalStorage:StaticPath'.");
            var generatedPath = builder.Configuration["LocalStorage:GeneratedPath"]
                ?? throw new InvalidOperationException("Missing configuration value 'LocalStorage:GeneratedPath'.");

            // Relative paths are resolved against the API project folder, so they work after cloning on any machine
            var storageSettings = new LocalStorageSettings(
                Path.GetFullPath(staticPath, contentRootPath),
                Path.GetFullPath(generatedPath, contentRootPath));

            Directory.CreateDirectory(storageSettings.GeneratedPath);
            builder.Services.AddSingleton(storageSettings);
        }
    }
}
