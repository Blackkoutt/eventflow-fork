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
            var section = builder.Configuration.GetSection("LocalStorage");
            var staticPath = ResolveFromContentRoot(builder, GetRequiredPath(section, "StaticPath"));
            var generatedPath = ResolveFromContentRoot(builder, GetRequiredPath(section, "GeneratedPath"));

            Directory.CreateDirectory(generatedPath);
            builder.Services.AddSingleton(new LocalStorageSettings(staticPath, generatedPath));
        }

        private static string GetRequiredPath(IConfigurationSection section, string key) =>
            section[key] ?? throw new InvalidOperationException($"Missing configuration value 'LocalStorage:{key}'.");

        // Relative paths are resolved against the API project folder, so they work after cloning on any machine
        private static string ResolveFromContentRoot(WebApplicationBuilder builder, string path) =>
            Path.GetFullPath(path, builder.Environment.ContentRootPath);
    }
}
