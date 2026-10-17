using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Authorization
{
    [Collection(ReadOnlyApiCollection.Name)]
    public class AuthorizationMatrixTests
    {
        private const string StatisticsPdfRoute = "api/Statistics/pdf-report";

        private static readonly string[] ExpectedAccess =
        {
            "GET /api/AdditionalServices => public",
            "POST /api/AdditionalServices => admin",
            "DELETE /api/AdditionalServices/{id:int} => admin",
            "GET /api/AdditionalServices/{id:int} => public",
            "PUT /api/AdditionalServices/{id:int} => admin",
            "POST /api/Auth/login => public",
            "POST /api/Auth/register => public",
            "GET /api/Auth/validate => authenticated",
            "GET /api/Equipments => public",
            "POST /api/Equipments => admin",
            "DELETE /api/Equipments/{id:int} => admin",
            "GET /api/Equipments/{id:int} => public",
            "PUT /api/Equipments/{id:int} => admin",
            "GET /api/EventCategories => public",
            "POST /api/EventCategories => admin",
            "DELETE /api/EventCategories/{id:int} => admin",
            "GET /api/EventCategories/{id:int} => public",
            "PUT /api/EventCategories/{id:int} => admin",
            "GET /api/EventPassTypes => public",
            "POST /api/EventPassTypes => admin",
            "DELETE /api/EventPassTypes/{id:int} => admin",
            "GET /api/EventPassTypes/{id:int} => public",
            "PUT /api/EventPassTypes/{id:int} => admin",
            "GET /api/EventPasses => authenticated",
            "POST /api/EventPasses => authenticated",
            "DELETE /api/EventPasses/{id:int} => admin",
            "GET /api/EventPasses/{id:int} => authenticated",
            "PUT /api/EventPasses/{id:int} => authenticated",
            "GET /api/EventPasses/{id:int}/jpg => authenticated",
            "GET /api/EventPasses/{id:int}/pdf => authenticated",
            "GET /api/Events => public",
            "POST /api/Events => admin",
            "DELETE /api/Events/{id:int} => admin",
            "GET /api/Events/{id:int} => public",
            "PUT /api/Events/{id:int} => admin",
            "PUT /api/Events/{id:int}/hall => admin",
            "GET /api/Events/{id:int}/image => public",
            "GET /api/Events/{id:int}/pdf-hallview => authenticated",
            "GET /api/FAQ => public",
            "POST /api/FAQ => admin",
            "DELETE /api/FAQ/{id:int} => admin",
            "GET /api/FAQ/{id:int} => public",
            "PUT /api/FAQ/{id:int} => admin",
            "GET /api/Festivals => public",
            "POST /api/Festivals => admin",
            "DELETE /api/Festivals/{id:int} => admin",
            "GET /api/Festivals/{id:int} => public",
            "PUT /api/Festivals/{id:int} => admin",
            "GET /api/Festivals/{id:int}/image => public",
            "GET /api/HallRents => public",
            "POST /api/HallRents => authenticated",
            "DELETE /api/HallRents/{id:int} => authenticated",
            "GET /api/HallRents/{id:int} => authenticated",
            "PUT /api/HallRents/{id:int}/hall => authenticated",
            "GET /api/HallRents/{id:int}/pdf-hallview => authenticated",
            "GET /api/HallRents/{id:int}/pdf-invoice => authenticated",
            "GET /api/HallTypes => public",
            "POST /api/HallTypes => admin",
            "DELETE /api/HallTypes/{id:int} => admin",
            "GET /api/HallTypes/{id:int} => public",
            "PUT /api/HallTypes/{id:int} => admin",
            "GET /api/HallTypes/{id:int}/image => public",
            "GET /api/Halls => public",
            "POST /api/Halls => authenticated",
            "DELETE /api/Halls/{id:int} => admin",
            "GET /api/Halls/{id:int} => public",
            "PUT /api/Halls/{id:int} => admin",
            "GET /api/Halls/{id:int}/pdf-hallview => authenticated",
            "GET /api/MediaPatrons => public",
            "POST /api/MediaPatrons => admin",
            "DELETE /api/MediaPatrons/{id:int} => admin",
            "GET /api/MediaPatrons/{id:int} => public",
            "PUT /api/MediaPatrons/{id:int} => admin",
            "GET /api/MediaPatrons/{id:int}/image => public",
            "GET /api/News => public",
            "POST /api/News => admin",
            "DELETE /api/News/{id:int} => admin",
            "GET /api/News/{id:int} => public",
            "PUT /api/News/{id:int} => admin",
            "GET /api/News/{id:int}/image => public",
            "GET /api/Organizers => public",
            "POST /api/Organizers => admin",
            "DELETE /api/Organizers/{id:int} => admin",
            "GET /api/Organizers/{id:int} => public",
            "PUT /api/Organizers/{id:int} => admin",
            "GET /api/Organizers/{id:int}/image => public",
            "GET /api/Partners => public",
            "POST /api/Partners => admin",
            "DELETE /api/Partners/{id:int} => admin",
            "GET /api/Partners/{id:int} => public",
            "PUT /api/Partners/{id:int} => admin",
            "GET /api/Partners/{id:int}/image => public",
            "GET /api/PaymentTypes => public",
            "POST /api/PaymentTypes => admin",
            "DELETE /api/PaymentTypes/{id:int} => admin",
            "GET /api/PaymentTypes/{id:int} => public",
            "GET /api/PaymentTypes/{id:int}/image => public",
            "GET /api/Reservations => public",
            "POST /api/Reservations => authenticated",
            "DELETE /api/Reservations/{id:int} => authenticated",
            "GET /api/Reservations/{id:int} => authenticated",
            "GET /api/Reservations/{id:int}/jpg-tickets => authenticated",
            "GET /api/Reservations/{id:int}/pdf-ticket => authenticated",
            "GET /api/SeatTypes => public",
            "POST /api/SeatTypes => admin",
            "DELETE /api/SeatTypes/{id:int} => admin",
            "GET /api/SeatTypes/{id:int} => public",
            "PUT /api/SeatTypes/{id:int} => admin",
            "GET /api/Sponsors => public",
            "POST /api/Sponsors => admin",
            "DELETE /api/Sponsors/{id:int} => admin",
            "GET /api/Sponsors/{id:int} => public",
            "PUT /api/Sponsors/{id:int} => admin",
            "GET /api/Sponsors/{id:int}/image => public",
            "GET /api/Statistics => admin",
            "GET /api/Statistics/pdf-report => admin",
            "GET /api/TicketTypes => public",
            "POST /api/TicketTypes => admin",
            "DELETE /api/TicketTypes/{id:int} => admin",
            "GET /api/TicketTypes/{id:int} => public",
            "PUT /api/TicketTypes/{id:int} => admin",
            "GET /api/Users => admin",
            "GET /api/Users/info => authenticated",
            "PUT /api/Users/info => public",
            "GET /api/Users/info/image => public",
            "GET /api/Users/{id} => admin",
            "GET /api/Users/{id}/image => public",
        };

        private readonly ApiFactory _factory;
        private readonly EndpointCatalog _catalog;

        public AuthorizationMatrixTests(ApiFactory factory)
        {
            _factory = factory;
            _catalog = new EndpointCatalog(factory.Services);
        }

        [Fact]
        public void Access_level_of_every_endpoint_is_as_expected()
        {
            var actual = _catalog.GetAll().Select(endpoint => endpoint.Describe()).ToList();

            actual.Except(ExpectedAccess).ShouldBeEmpty("endpoints with an unexpected or new access level");
            ExpectedAccess.Except(actual).ShouldBeEmpty("expected endpoints that no longer exist");
            actual.Count.ShouldBe(ExpectedAccess.Length);
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
