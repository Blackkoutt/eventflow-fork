using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class ValidationTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public ValidationTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        [Fact]
        public async Task Event_longer_than_24_hours_returns_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();
            var form = _world.EventForm(_world.UniqueName("Wydarzenie"), 4, start, start.AddHours(30), (10, ApiLookups.NormalTicketTypeId));

            var response = await admin.PostFormAsync("/api/Events", form);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadJsonAsync(response)).GetRawText().ShouldContain("24");
        }

        [Fact]
        public async Task Hall_rent_longer_than_24_hours_returns_400()
        {
            var user = await _world.RegisterUserAsync();

            var response = await _world.RentAsync(user.Client, _world.NextFarStart(), 30);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await user.Client.ReadJsonAsync(response)).GetRawText().ShouldContain("24");
        }

        [Fact]
        public async Task Photo_larger_than_10_MB_is_currently_accepted_because_the_size_validator_never_fires()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var big = new byte[10 * 1024 * 1024 + 1];
            var form = new FormData().Add("Name", _world.UniqueName("Patron")).AddFile("MediaPatronPhoto", "big.png", big, "image/png");

            var response = await admin.PostFormAsync("/api/MediaPatrons", form);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Unsupported_method_returns_405()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.SendAsync("PATCH", "/api/Events/1");

            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }
    }
}
