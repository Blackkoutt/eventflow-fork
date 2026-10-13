using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Authorization
{
    [Collection(ReadOnlyApiCollection.Name)]
    public class AuthorizationMatrixTests
    {
        private const string StatisticsPdfRoute = "api/Statistics/pdf-report";

        private readonly ApiFactory _factory;
        private readonly EndpointCatalog _catalog;

        public AuthorizationMatrixTests(ApiFactory factory)
        {
            _factory = factory;
            _catalog = new EndpointCatalog(factory.Services);
        }

        [Fact]
        public async Task Access_level_of_every_endpoint_matches_snapshot()
        {
            var lines = _catalog.GetAll().Select(endpoint => endpoint.Describe());

            await Verifier.Verify(string.Join("\n", lines));
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

            foreach (var endpoint in _catalog.GetAll().Where(endpoint => endpoint.Method == "GET" && endpoint.Route != StatisticsPdfRoute))
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
