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
            var staticPath = ResolveFromContentRoot(builder, section["StaticPath"] ?? "../EventFlowAPI.DB/Assets/static");
            var generatedPath = ResolveFromContentRoot(builder, section["GeneratedPath"] ?? "../EventFlowAPI.DB/Assets/generated");

            Directory.CreateDirectory(generatedPath);
            builder.Services.AddSingleton(new LocalStorageSettings(staticPath, generatedPath));
        }

        // Relative paths are resolved against the API project folder, so they work after cloning on any machine
        private static string ResolveFromContentRoot(WebApplicationBuilder builder, string path) =>
            Path.GetFullPath(path, builder.Environment.ContentRootPath);
    }
}
