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
        public async Task Event_with_a_rejected_photo_currently_leaves_an_orphan_hall_copy()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();
            var form = _world.EventForm(_world.UniqueName("Wydarzenie"), 1, start, start.AddHours(2), (10, 1))
                .AddFile("EventPhoto", "photo.txt", TestImages.Text, "text/plain");

            var changes = await ChangesAsync(() => admin.PostFormAsync("/api/Events", form), HttpStatusCode.BadRequest);

            changes.ShouldBe(new[] { "Hall +1", "HallDetails +1", "Seat +4" });
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
        public async Task Rejected_festival_leaves_no_rows()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var form = _world.FestivalForm(_world.UniqueName("Festiwal"), new[] { 7, 8 }, 1, (50, 1));

            var changes = await ChangesAsync(() => admin.PostFormAsync("/api/Festivals", form), HttpStatusCode.BadRequest);

            changes.ShouldBeEmpty();
        }

        [Fact]
        public async Task Duplicate_lookup_leaves_no_rows()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var duplicate = new { name = "DJ", price = 10, description = "opis" };

            var changes = await ChangesAsync(() => admin.PostAsync("/api/AdditionalServices", duplicate), HttpStatusCode.InternalServerError);

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

        [Fact]
        public async Task Failed_delete_of_a_seeded_hall_currently_commits_part_of_the_cascade()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var lookups = new ApiLookups(admin);
            var before = await admin.GetJsonAsync("/api/Events?Status=Active");

            var changes = await ChangesAsync(() => admin.DeleteAsync("/api/Halls/1"), HttpStatusCode.InternalServerError);

            var after = await admin.GetJsonAsync("/api/Events?Status=Active");
            changes.ShouldBe(new[] { "Seat -4" });
            lookups.ReadIds(after).ShouldBe(lookups.ReadIds(before));
        }
    }
}
