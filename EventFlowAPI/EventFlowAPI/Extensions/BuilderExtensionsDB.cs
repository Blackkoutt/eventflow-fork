using EventFlowAPI.Data.SqlServer;
using EventFlowAPI.Enums;
using EventFlowAPI.Logic.Helpers;

namespace EventFlowAPI.Extensions
{
    public static class BuilderExtensionsDB
    {
        /// <summary>
        /// Registers the data layer chosen by the "Database:Provider" setting (SqlServer when not set).
        /// </summary>
        public static void AddDatabase(this WebApplicationBuilder builder)
        {
            var providerName = builder.Configuration["Database:Provider"] ?? nameof(DatabaseProvider.SqlServer);
            if (!Enum.TryParse<DatabaseProvider>(providerName, ignoreCase: true, out var provider))
            {
                throw new InvalidOperationException(
                    $"Unknown database provider '{providerName}'. Allowed values: {string.Join(", ", Enum.GetNames<DatabaseProvider>())}.");
            }

            switch (provider)
            {
                case DatabaseProvider.SqlServer:
                    builder.Services.AddSqlServerDatabase(GetRequiredConnectionString(builder, "MSSQLEventFlowDB"));
                    break;
                default:
                    throw new NotSupportedException($"The {provider} database provider is not implemented yet.");
            }
        }

        private static string GetRequiredConnectionString(WebApplicationBuilder builder, string name)
            => builder.Configuration.GetConnectionString(name)
               ?? throw new InvalidOperationException($"Missing connection string '{name}'.");

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
