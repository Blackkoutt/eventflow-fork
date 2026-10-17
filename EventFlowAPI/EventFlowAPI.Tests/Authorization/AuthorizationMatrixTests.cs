using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Authorization
{
    [Collection(ReadOnlyApiCollection.Name)]
    public class AuthorizationMatrixTests
    {
        // Rendering PDFs and images is slow and covered by GeneratedFileTests.
        private static bool IsGeneratedFile(EndpointInfo endpoint) => endpoint.Route.Contains("pdf") || endpoint.Route.Contains("jpg");

        private readonly ApiFactory _factory;
        private readonly EndpointCatalog _catalog;

        public AuthorizationMatrixTests(ApiFactory factory)
        {
            _factory = factory;
            _catalog = new EndpointCatalog(factory.Services);
        }

        [Fact]
        public void Only_login_register_and_user_info_update_are_currently_public_write_endpoints()
        {
            var publicWrites = _catalog.GetAll()
                .Where(endpoint => endpoint.Method != "GET" && endpoint.Access == EndpointCatalog.PublicAccess)
                .Select(endpoint => $"{endpoint.Method} /{endpoint.Route}");

            // PUT /api/Users/info has no [Authorize] today; remove it from this list together with the fix.
            publicWrites.ShouldBe(new[] { "POST /api/Auth/login", "POST /api/Auth/register", "PUT /api/Users/info" }, ignoreOrder: true);
        }

        [Theory]
        [InlineData("api/Users", EndpointCatalog.AdminAccess)]
        [InlineData("api/Users/{id}", EndpointCatalog.AdminAccess)]
        [InlineData("api/Users/info", EndpointCatalog.AuthenticatedAccess)]
        [InlineData("api/Statistics", EndpointCatalog.AdminAccess)]
        [InlineData("api/Statistics/pdf-report", EndpointCatalog.AdminAccess)]
        [InlineData("api/Reservations/{id:int}", EndpointCatalog.AuthenticatedAccess)]
        [InlineData("api/HallRents/{id:int}", EndpointCatalog.AuthenticatedAccess)]
        [InlineData("api/EventPasses", EndpointCatalog.AuthenticatedAccess)]
        [InlineData("api/EventPasses/{id:int}", EndpointCatalog.AuthenticatedAccess)]
        public void Personal_and_admin_reads_are_not_public(string route, string expectedAccess)
        {
            var endpoint = _catalog.GetAll().Single(candidate => candidate.Method == "GET" && candidate.Route == route);

            endpoint.Access.ShouldBe(expectedAccess);
        }

        [Fact]
        public async Task Anonymous_caller_gets_401_on_every_protected_endpoint()
        {
            var client = _factory.CreateAnonymousClient();
            var failures = new List<string>();

            foreach (var endpoint in _catalog.GetAll().Where(endpoint => endpoint.Access != EndpointCatalog.PublicAccess))
            {
                var response = await client.SendAsync(endpoint.Method, _catalog.ToConcreteUrl(endpoint));
                if (response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    failures.Add($"{endpoint.Describe()} returned {(int)response.StatusCode}");
                }
            }

            failures.ShouldBeEmpty();
        }

        [Fact]
        public async Task Regular_user_gets_403_on_every_admin_endpoint()
        {
            var client = await _factory.CreateClientAsync(TestUsers.User5);
            var failures = new List<string>();

            foreach (var endpoint in _catalog.GetAll().Where(endpoint => endpoint.Access == EndpointCatalog.AdminAccess))
            {
                var response = await client.SendAsync(endpoint.Method, _catalog.ToConcreteUrl(endpoint));
                if (response.StatusCode != HttpStatusCode.Forbidden)
                {
                    failures.Add($"{endpoint.Describe()} returned {(int)response.StatusCode}");
                }
            }

            failures.ShouldBeEmpty();
        }

        [Fact]
        public async Task Anonymous_caller_can_reach_every_public_read_endpoint()
        {
            var client = _factory.CreateAnonymousClient();
            var failures = new List<string>();

            foreach (var endpoint in _catalog.GetAll().Where(endpoint => endpoint.Access == EndpointCatalog.PublicAccess && endpoint.Method == "GET"))
            {
                var response = await client.SendAsync(endpoint.Method, _catalog.ToConcreteUrl(endpoint));
                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                {
                    failures.Add($"{endpoint.Describe()} returned {(int)response.StatusCode}");
                }
            }

            failures.ShouldBeEmpty();
        }

        [Fact]
        public async Task Admin_is_not_rejected_on_any_read_endpoint()
        {
            var client = await _factory.CreateClientAsync(TestUsers.Admin);
            var failures = new List<string>();

            foreach (var endpoint in _catalog.GetAll().Where(endpoint => endpoint.Method == "GET" && !IsGeneratedFile(endpoint)))
            {
                try
                {
                    var response = await client.SendAsync(endpoint.Method, _catalog.ToConcreteUrl(endpoint));
                    if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        failures.Add($"{endpoint.Describe()} returned {(int)response.StatusCode}");
                    }
                }
                catch (TaskCanceledException)
                {
                    failures.Add($"{endpoint.Describe()} timed out");
                }
            }

            failures.ShouldBeEmpty();
        }
    }
}
