using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class TicketUpdateTests : IClassFixture<ApiFactory>
    {
        private const int Normal = ApiLookups.NormalTicketTypeId;
        private const int Reduced = ApiLookups.ReducedTicketTypeId;

        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public TicketUpdateTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private FormData FestivalForm(IEnumerable<int> eventIds, params (int Price, int TicketTypeId)[] tickets)
        {
            return _world.FestivalForm(_world.UniqueName("Festiwal"), eventIds, 1, tickets);
        }

        private FormData EventForm(DateTime start, params (int Price, int TicketTypeId)[] tickets)
        {
            return _world.EventForm(_world.UniqueName("Wydarzenie"), 1, start, start.AddHours(2), tickets);
        }

        [Fact]
        public async Task Updating_festival_tickets_changes_price_adds_a_type_and_removes_a_type()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var lookups = new ApiLookups(admin);
            var (festivalId, eventIds) = await _world.CreateFestivalAsync();
            var url = $"/api/Festivals/{festivalId}";

            var priceChange = await admin.PutFormAsync(url, FestivalForm(eventIds, (60, Normal)));
            var afterPrice = lookups.ReadTickets(await admin.GetJsonAsync(url));
            var newType = await admin.PutFormAsync(url, FestivalForm(eventIds, (60, Normal), (40, Reduced)));
            var afterAdd = lookups.ReadTickets(await admin.GetJsonAsync(url));
            var removeType = await admin.PutFormAsync(url, FestivalForm(eventIds, (60, Normal)));
            var afterRemove = lookups.ReadTickets(await admin.GetJsonAsync(url));

            priceChange.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            newType.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            removeType.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            afterPrice.ShouldBe(new[] { (Normal, 60m), (Normal, 60m) });
            afterAdd.ShouldBe(new[] { (Normal, 60m), (Normal, 60m), (Reduced, 40m), (Reduced, 40m) });
            afterRemove.ShouldBe(new[] { (Normal, 60m), (Normal, 60m) });
        }

        [Fact]
        public async Task Updating_event_tickets_changes_price_and_removes_a_type_but_currently_ignores_re_adding_one()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var lookups = new ApiLookups(admin);
            var eventId = await _world.CreateEventAsync();
            var url = $"/api/Events/{eventId}";
            var start = _world.NextFarStart();

            await admin.PutFormAsync(url, EventForm(start, (35, Normal), (20, Reduced)));
            var afterPrice = lookups.ReadTickets(await admin.GetJsonAsync(url));
            await admin.PutFormAsync(url, EventForm(start, (35, Normal)));
            var afterRemove = lookups.ReadTickets(await admin.GetJsonAsync(url));
            await admin.PutFormAsync(url, EventForm(start, (35, Normal), (15, Reduced)));
            var afterAdd = lookups.ReadTickets(await admin.GetJsonAsync(url));

            afterPrice.ShouldBe(new[] { (Normal, 35m), (Reduced, 20m) });
            afterRemove.ShouldBe(new[] { (Normal, 35m) });
            afterAdd.ShouldBe(new[] { (Normal, 35m) });
        }

        [Fact]
        public async Task Removing_an_event_ticket_type_that_has_reservations_currently_keeps_it_listed()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var lookups = new ApiLookups(user.Client);
            var eventId = await _world.CreateEventAsync();
            var ticketId = await lookups.GetTicketIdAsync($"/api/Events/{eventId}", Reduced);
            var seats = await lookups.GetSeatIdsAsync(eventId);
            var reserve = await _world.ReserveAsync(user.Client, ticketId, new[] { seats[0] });
            var reservationId = (await user.Client.ReadJsonAsync(reserve))[0].GetProperty("id").GetInt32();
            var start = (await admin.GetJsonAsync($"/api/Events/{eventId}")).GetProperty("start").GetDateTime();

            var response = await admin.PutFormAsync($"/api/Events/{eventId}", EventForm(start, (30, Normal)));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await admin.ReadErrorsAsync(response));
            lookups.ReadTickets(await admin.GetJsonAsync($"/api/Events/{eventId}")).ShouldBe(new[] { (Normal, 30m), (Reduced, 20m) });
            (await lookups.GetStatusAsync($"/api/Reservations/{reservationId}", "reservationStatus")).ShouldBe("Active");
        }
    }
}
