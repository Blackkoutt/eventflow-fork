using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class FestivalTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public FestivalTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private FormData Form(IEnumerable<int> eventIds, string? name = null, int partnerId = 1)
        {
            return _world.FestivalForm(name ?? _world.UniqueName("Festiwal"), eventIds, partnerId, (50, ApiLookups.NormalTicketTypeId));
        }

        [Fact]
        public async Task Creating_a_festival_links_events_patrons_organizers_sponsors_and_tickets()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var (festivalId, eventIds) = await _world.CreateFestivalAsync();

            var festival = await admin.GetJsonAsync($"/api/Festivals/{festivalId}");
            new ApiLookups(admin).ReadIds(festival.GetProperty("events")).ShouldBe(eventIds, ignoreOrder: true);
            festival.GetProperty("mediaPatrons").GetArrayLength().ShouldBe(1);
            festival.GetProperty("organizers").GetArrayLength().ShouldBe(1);
            festival.GetProperty("sponsors").GetArrayLength().ShouldBe(1);
            festival.GetProperty("tickets").GetArrayLength().ShouldBe(2);
            festival.GetProperty("festivalStatus").GetString().ShouldBe("Active");
        }

        [Fact]
        public async Task Festival_needs_at_least_two_events()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var eventId = await _world.CreateEventAsync(soon: true);

            var response = await admin.PostFormAsync("/api/Festivals", Form(new[] { eventId }));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(response)).ShouldBe("Festiwal musi zawierać co najmniej 2 wydarzenia.");
        }

        [Fact]
        public async Task Festival_rejects_duplicated_and_unknown_events()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var eventId = await _world.CreateEventAsync(soon: true);

            var duplicated = await admin.PostFormAsync("/api/Festivals", Form(new[] { eventId, eventId }));
            var unknown = await admin.PostFormAsync("/api/Festivals", Form(new[] { eventId, 999 }));

            duplicated.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(duplicated)).ShouldBe("Lista ID wydarzeń zawiera duplikaty.");
            unknown.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(unknown)).ShouldBe("Wydarzenie o podanym ID nie istnieje w bazie danych.");
        }

        [Fact]
        public async Task Events_more_than_48_hours_apart_cannot_form_a_festival()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.PostFormAsync("/api/Festivals", Form(new[] { 7, 8 }));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(response)).ShouldBe("Maksymalny czas wolny pomiędzy wydarzeniami w festiwalu wynosi 48 godzin.");
        }

        [Fact]
        public async Task Updating_a_festival_replaces_its_patrons()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var (festivalId, eventIds) = await _world.CreateFestivalAsync();

            var response = await admin.PutFormAsync($"/api/Festivals/{festivalId}", Form(eventIds, "Festiwal zmieniony", partnerId: 2));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var festival = await admin.GetJsonAsync($"/api/Festivals/{festivalId}");
            festival.GetProperty("title").GetString().ShouldBe("Festiwal zmieniony");
            festival.GetProperty("mediaPatrons")[0].GetProperty("id").GetInt32().ShouldBe(2);
            festival.GetProperty("organizers")[0].GetProperty("id").GetInt32().ShouldBe(2);
            festival.GetProperty("sponsors")[0].GetProperty("id").GetInt32().ShouldBe(2);
        }

        [Fact]
        public async Task Deleting_a_festival_keeps_it_readable_with_a_changed_status()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var (festivalId, _) = await _world.CreateFestivalAsync();

            var response = await admin.DeleteAsync($"/api/Festivals/{festivalId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var after = await admin.GetAsync($"/api/Festivals/{festivalId}");
            after.StatusCode.ShouldBe(HttpStatusCode.OK, await admin.ReadErrorsAsync(after));
            (await admin.ReadJsonAsync(after)).GetProperty("festivalStatus").GetString().ShouldBe("Canceled");
        }

        [Fact]
        public async Task Deleting_a_festival_cancels_reservations_made_for_it()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var before = await admin.GetJsonAsync("/api/Reservations/5");

            var response = await admin.DeleteAsync("/api/Festivals/1");

            before.GetProperty("reservationStatus").GetString().ShouldBe("Active");
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            (await admin.GetJsonAsync("/api/Reservations/5")).GetProperty("reservationStatus").GetString().ShouldBe("Canceled");
        }

        [Fact]
        public async Task Festival_image_is_served()
        {
            var image = await _factory.CreateAnonymousClient().GetAsync("/api/Festivals/3/image");

            image.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
