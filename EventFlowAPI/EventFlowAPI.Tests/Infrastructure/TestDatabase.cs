using Microsoft.Data.SqlClient;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class TestDatabase
    {
        public const string Collation = "Polish_CI_AS";

        private readonly string _serverConnectionString;

        public string Name { get; }
        public string ConnectionString { get; }

        public TestDatabase()
        {
            _serverConnectionString = Environment.GetEnvironmentVariable("EVENTFLOW_TEST_SQL")
                ?? "Server=.;Integrated Security=True;TrustServerCertificate=True";
            Name = $"EventFlowTests_{Guid.NewGuid():N}".Substring(0, 28);
            ConnectionString = $"{_serverConnectionString};Database={Name};Connect Timeout=60;Max Pool Size=1000";
        }

        public async Task CreateAsync()
        {
            await using var connection = new SqlConnection(_serverConnectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = $"CREATE DATABASE [{Name}] COLLATE {Collation}";
            await command.ExecuteNonQueryAsync();
        }

        public async Task DropAsync()
        {
            SqlConnection.ClearAllPools();
            await DropAsync(Name);
        }

        public async Task DropStaleAsync()
        {
            var staleNames = new List<string>();
            await using (var connection = new SqlConnection(_serverConnectionString))
            {
                await connection.OpenAsync();
                await using var command = connection.CreateCommand();
                command.CommandText =
                    "SELECT name FROM sys.databases WHERE name LIKE 'EventFlowTests[_]%' AND create_date < DATEADD(hour, -1, GETDATE())";
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    staleNames.Add(reader.GetString(0));
                }
            }

            foreach (var name in staleNames)
            {
                await DropAsync(name);
            }
        }

        private async Task DropAsync(string name)
        {
            await using var connection = new SqlConnection(_serverConnectionString);
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"IF DB_ID('{name}') IS NOT NULL BEGIN ALTER DATABASE [{name}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{name}]; END";
            await command.ExecuteNonQueryAsync();
        }
    }
}
