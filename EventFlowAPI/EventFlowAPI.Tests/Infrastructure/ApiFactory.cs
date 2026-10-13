using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using EventFlowAPI.DB.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private const string JwtSigningKey = "TestOnlyJwtSigningKey_0123456789_ABCDEFGHIJKLMNOP";

        private readonly TestDatabase _database = new();
        private readonly string _generatedFilesDirectory =
            Path.Combine(Path.GetTempPath(), "eventflow-tests-" + Guid.NewGuid().ToString("N"));

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Directory.SetCurrentDirectory(new RepositoryPaths().ApiProjectDirectory);
            builder.UseEnvironment("Development");
            builder.UseSetting("ConnectionStrings:MSSQLEventFlowDB", _database.ConnectionString);
            builder.UseSetting("JWTAuth:securityKey", JwtSigningKey);
            builder.UseSetting("LocalStorage:GeneratedPath", _generatedFilesDirectory);
        }

        public async Task InitializeAsync()
        {
            await _database.DropStaleAsync();
            await _database.CreateAsync();
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<APIContext>();
            await context.Database.MigrateAsync();
            await new SeedDateShifter().ShiftToTodayAsync(context);
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await base.DisposeAsync();
            await _database.DropAsync();
            if (Directory.Exists(_generatedFilesDirectory))
            {
                Directory.Delete(_generatedFilesDirectory, recursive: true);
            }
        }

        private int _slot;

        public int NextSlot()
        {
            return Interlocked.Increment(ref _slot);
        }

        public new HttpClient CreateClient()
        {
            var httpClient = base.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            return httpClient;
        }

        public ApiClient CreateAnonymousClient()
        {
            return new ApiClient(CreateClient());
        }

        public async Task<ApiClient> CreateClientForRoleAsync(string role)
        {
            switch (role)
            {
                case "anonymous":
                    return CreateAnonymousClient();
                case "admin":
                    return await CreateClientAsync(TestUsers.Admin);
                case "user2":
                    return await CreateClientAsync(TestUsers.User2);
                case "user3":
                    return await CreateClientAsync(TestUsers.User3);
                case "user4":
                    return await CreateClientAsync(TestUsers.User4);
                case "user5":
                    return await CreateClientAsync(TestUsers.User5);
                default:
                    throw new ArgumentException($"Unknown role {role}");
            }
        }

        public async Task<ApiClient> CreateClientAsync(TestUser user)
        {
            var httpClient = CreateClient();
            var response = await httpClient.PostAsJsonAsync("/api/Auth/login", new { email = user.Email, password = user.Password });
            response.EnsureSuccessStatusCode();

            var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            var token = body.GetProperty("token").GetString();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return new ApiClient(httpClient);
        }
    }
}
