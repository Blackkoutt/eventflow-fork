using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    [Trait(TestCategories.Name, TestCategories.DatabaseSpecific)]
    public class AtomicityTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;
        private readonly DatabaseProbe _probe;

        public AtomicityTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
            _probe = new DatabaseProbe(factory.Services);
        }

        private async Task<List<string>> ChangesAsync(Func<Task<HttpResponseMessage>> request, HttpStatusCode expectedStatus)
        {
            HttpResponseMessage? response = null;
            var changes = await _probe.ChangesDuringAsync(async () =>
            {
                response = await request();
            });
            response!.StatusCode.ShouldBe(expectedStatus);
            return changes;
        }

        [Fact]
        public async Task Colliding_event_leaves_no_rows()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();
            var form = _world.EventForm(_world.UniqueName("Wydarzenie"), 1, start, start.AddHours(2), (10, 1));
            (await admin.PostFormAsync("/api/Events", form)).StatusCode.ShouldBe(HttpStatusCode.Created);
            var colliding = _world.EventForm(_world.UniqueName("Wydarzenie"), 1, start.AddHours(1), start.AddHours(3), (10, 1));

            var changes = await ChangesAsync(() => admin.PostFormAsync("/api/Events", colliding), HttpStatusCode.BadRequest);

            changes.ShouldBeEmpty();
        }

        [Fact]
        public async Task Reserving_a_taken_seat_leaves_no_rows()
        {
            var first = await _world.RegisterUserAsync();
            var second = await _world.RegisterUserAsync();
            var eventId = await _world.CreateEventAsync();
            var lookups = new ApiLookups(first.Client);
            var ticketId = await lookups.GetNormalTicketIdAsync(eventId);
            var seats = await lookups.GetSeatIdsAsync(eventId);
            await _world.ReserveSeatAsync(first.Client, eventId);

            var changes = await ChangesAsync(() => _world.ReserveAsync(second.Client, ticketId, new[] { seats[1], seats[0] }), HttpStatusCode.BadRequest);

            changes.ShouldBeEmpty();
        }

        [Fact]
        public async Task Failed_delete_of_an_event_with_reservations_leaves_no_changes()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var changes = await ChangesAsync(() => admin.DeleteAsync("/api/Events/2"), HttpStatusCode.InternalServerError);

            changes.ShouldBeEmpty();
            (await new ApiLookups(admin).GetStatusAsync("/api/Events/2", "eventStatus")).ShouldBe("Active");
            (await new ApiLookups(admin).GetStatusAsync("/api/Reservations/2", "reservationStatus")).ShouldBe("Active");
        }
    }
}
