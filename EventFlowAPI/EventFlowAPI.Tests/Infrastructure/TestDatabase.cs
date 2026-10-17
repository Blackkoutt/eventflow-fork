using Microsoft.Data.SqlClient;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class TestDatabase
    {
        public const string Collation = "Polish_CI_AS";

        private string _serverConnectionString = "";

        public string Name { get; }
        public string ConnectionString => new SqlConnectionStringBuilder(_serverConnectionString) { InitialCatalog = Name, ConnectTimeout = 60, MaxPoolSize = 1000 }.ConnectionString;

        public TestDatabase()
        {
            Name = $"EventFlowTests_{Guid.NewGuid():N}".Substring(0, 28);
        }

        private async Task EnsureServerAsync()
        {
            if (_serverConnectionString == "")
            {
                _serverConnectionString = await TestSqlServer.GetConnectionStringAsync();
            }
        }

        public async Task CreateAsync()
        {
            await EnsureServerAsync();
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
            await EnsureServerAsync();
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
