using Testcontainers.MsSql;

namespace EventFlowAPI.Tests.Infrastructure
{
    // One SQL Server per test run. Set EVENTFLOW_TEST_SQL (connection string without a database name) to use an
    // existing server such as LocalDB; otherwise a throw-away SQL Server container is started (requires Docker).
    public static class TestSqlServer
    {
        public const string ConnectionVariable = "EVENTFLOW_TEST_SQL";

        private static readonly Lazy<Task<string>> ConnectionString = new(StartAsync);

        public static Task<string> GetConnectionStringAsync()
        {
            return ConnectionString.Value;
        }

        private static async Task<string> StartAsync()
        {
            var configured = Environment.GetEnvironmentVariable(ConnectionVariable);
            if (!string.IsNullOrWhiteSpace(configured))
            {
                return configured;
            }

            var container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest").Build();
            await container.StartAsync();
            return container.GetConnectionString();
        }
    }
}
