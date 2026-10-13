using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class HallLayoutTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public HallLayoutTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private async Task<int> SeatCountAsync(ApiClient client, string ownerUrl)
        {
            var owner = await client.GetJsonAsync(ownerUrl);
            var hallId = owner.GetProperty("hall").GetProperty("id").GetInt32();
            return (await new ApiLookups(client).GetHallSeatIdsAsync(hallId)).Count;
        }

        [Fact]
        public async Task Event_hall_layout_can_be_replaced_when_nothing_is_reserved()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var eventId = await _world.CreateEventAsync();
            var layout = new { seats = _world.SeatRow(3) };

            var response = await admin.PutAsync($"/api/Events/{eventId}/hall", layout);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await admin.ReadErrorsAsync(response));
            (await SeatCountAsync(admin, $"/api/Events/{eventId}")).ShouldBe(3);
        }

        [Fact]
        public async Task Event_hall_layout_without_the_reserved_seat_is_rejected()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var eventId = await _world.CreateEventAsync();
            await _world.ReserveSeatAsync(user.Client, eventId);
            var layout = new { seats = Array.Empty<object>() };

            var response = await admin.PutAsync($"/api/Events/{eventId}/hall", layout);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await SeatCountAsync(admin, $"/api/Events/{eventId}")).ShouldBe(4);
        }

        [Fact]
        public async Task Hall_rent_layout_can_be_replaced_by_the_owner_only()
        {
            var owner = await _world.RegisterUserAsync();
            var stranger = await _world.RegisterUserAsync();
            var rentId = await _world.RentHallAsync(owner.Client);
            var layout = new { seats = _world.SeatRow(2) };

            var byOwner = await owner.Client.PutAsync($"/api/HallRents/{rentId}/hall", layout);
            var byStranger = await stranger.Client.PutAsync($"/api/HallRents/{rentId}/hall", layout);

            byOwner.StatusCode.ShouldBe(HttpStatusCode.NoContent, await owner.Client.ReadErrorsAsync(byOwner));
            byStranger.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            (await SeatCountAsync(owner.Client, $"/api/HallRents/{rentId}")).ShouldBe(2);
        }
    }
}
