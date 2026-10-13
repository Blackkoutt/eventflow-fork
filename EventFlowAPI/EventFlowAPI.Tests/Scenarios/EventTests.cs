using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class EventTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public EventTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private FormData Form(string name, int hallId, DateTime start, DateTime end, params (int Price, int TicketTypeId)[] tickets)
        {
            return _world.EventForm(name, hallId, start, end, tickets);
        }

        private string UniqueName()
        {
            return _world.UniqueName("Wydarzenie");
        }

        private async Task<HttpResponseMessage> CreateRawAsync(ApiClient admin, int hallId, DateTime start, DateTime end, params (int Price, int TicketTypeId)[] tickets)
        {
            return await admin.PostFormAsync("/api/Events", Form(UniqueName(), hallId, start, end, tickets));
        }

        [Fact]
        public async Task Creating_an_event_stores_tickets_category_and_a_copy_of_the_hall()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();

            var response = await CreateRawAsync(admin, 1, start, start.AddHours(2), (25, 1), (20, 2));

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            var created = await admin.ReadJsonAsync(response);
            var detail = await admin.GetJsonAsync($"/api/Events/{created.GetProperty("id").GetInt32()}");
            detail.GetProperty("eventStatus").GetString().ShouldBe("Active");
            detail.GetProperty("start").GetDateTime().ShouldBe(start);
            detail.GetProperty("category").GetProperty("id").GetInt32().ShouldBe(1);
            detail.GetProperty("tickets").GetArrayLength().ShouldBe(2);
            detail.GetProperty("hall").GetProperty("id").GetInt32().ShouldNotBe(1);
            detail.GetProperty("hall").GetProperty("hallNr").GetInt32().ShouldBe(1);
        }

        [Fact]
        public async Task Event_overlapping_another_event_in_the_same_hall_returns_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();
            (await CreateRawAsync(admin, 1, start, start.AddHours(2), (10, 1))).StatusCode.ShouldBe(HttpStatusCode.Created);

            var response = await CreateRawAsync(admin, 1, start.AddHours(1), start.AddHours(3), (10, 1));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(response)).ShouldStartWith("Wydarzenie koliduje z innym istniejącym wydarzeniem.");
        }

        [Fact]
        public async Task Event_overlapping_a_hall_rent_returns_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var start = _world.NextFarStart();
            var rent = await user.Client.PostAsync("/api/HallRents", new { startDate = start.ToString("s"), endDate = start.AddHours(3).ToString("s"), hallId = 2, additionalServicesIds = Array.Empty<int>() });
            rent.StatusCode.ShouldBe(HttpStatusCode.Created);

            var response = await CreateRawAsync(admin, 2, start.AddHours(1), start.AddHours(2), (10, 1));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(response)).ShouldStartWith("Wydarzenie koliduje z istniejącym wynajmem sali.");
        }

        [Fact]
        public async Task Event_overlapping_a_seeded_event_returns_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var seededStart = DateTime.Today.AddDays(34);

            var response = await CreateRawAsync(admin, 1, seededStart, seededStart.AddHours(1), (10, 1));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Duplicated_ticket_types_return_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();

            var response = await CreateRawAsync(admin, 3, start, start.AddHours(2), (10, 1), (11, 1));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(response)).ShouldBe("Lista ID typów biletów zawiera zduplikowane elementy.");
        }

        [Fact]
        public async Task Event_without_a_normal_ticket_returns_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();

            var response = await CreateRawAsync(admin, 4, start, start.AddHours(2));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(response)).ShouldBe("Konieczne jest podanie informacji o normalnym bilecie dla wydarzenia lub festiwalu.");
        }

        [Fact]
        public async Task Unknown_category_or_hall_returns_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();
            var badCategory = Form(UniqueName(), 1, start, start.AddHours(2), (10, 1)).Add("CategoryId", 999);

            var unknownHall = await CreateRawAsync(admin, 999, start, start.AddHours(2), (10, 1));
            var unknownCategory = await admin.PostFormAsync("/api/Events", new FormData().Add("Name", UniqueName()).Add("ShortDescription", "krotki").Add("CategoryId", 999)
                .Add("HallId", 1).Add("StartDate", start.ToString("s")).Add("EndDate", start.AddHours(2).ToString("s")));

            unknownHall.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(unknownHall)).ShouldBe("Sala wydarzenia o podanym ID nie istnieje w bazie danych.");
            unknownCategory.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(unknownCategory)).ShouldBe("Kategoria wydarzenia o podanym ID nie istnieje w bazie danych.");
        }

        [Fact]
        public async Task Event_in_the_past_or_ending_before_it_starts_returns_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();

            var past = await CreateRawAsync(admin, 2, DateTime.Today.AddDays(-10).AddHours(18), DateTime.Today.AddDays(-10).AddHours(20), (10, 1));
            var reversed = await CreateRawAsync(admin, 2, start.AddHours(2), start, (10, 1));

            past.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            reversed.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Updating_an_event_changes_its_data_and_replaces_tickets()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var id = await _world.CreateEventAsync();
            var start = _world.NextFarStart();

            var response = await admin.PutFormAsync($"/api/Events/{id}", Form("Zmienione wydarzenie", 1, start, start.AddHours(2), (30, 1)));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var detail = await admin.GetJsonAsync($"/api/Events/{id}");
            detail.GetProperty("title").GetString().ShouldBe("Zmienione wydarzenie");
            detail.GetProperty("start").GetDateTime().ShouldBe(start);
            detail.GetProperty("tickets").GetArrayLength().ShouldBe(1);
            detail.GetProperty("tickets")[0].GetProperty("price").GetDecimal().ShouldBe(30m);
        }

        [Fact]
        public async Task Updating_an_unknown_event_currently_returns_500_with_a_404_body()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();

            var response = await admin.PutFormAsync("/api/Events/9999", Form(UniqueName(), 1, start, start.AddHours(2), (30, 1)));

            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            (await admin.ReadJsonAsync(response)).GetProperty("code").GetInt32().ShouldBe(404);
        }

        [Fact]
        public async Task Expired_event_cannot_be_updated_or_deleted()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = _world.NextFarStart();

            var update = await admin.PutFormAsync("/api/Events/9", Form(UniqueName(), 2, start, start.AddHours(2), (30, 1)));
            var delete = await admin.DeleteAsync("/api/Events/9");

            update.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            delete.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Deleting_an_event_without_reservations_succeeds()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var id = await _world.CreateEventAsync();

            var response = await admin.DeleteAsync($"/api/Events/{id}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var deleted = await admin.GetJsonAsync($"/api/Events/{id}");
            deleted.GetProperty("eventStatus").GetString().ShouldBe("Canceled");
            new ApiLookups(admin).ReadIds(await admin.GetJsonAsync("/api/Events?Status=Active")).ShouldNotContain(id);
        }

        [Fact]
        public async Task Event_image_and_hall_view_pdf_are_served_for_a_new_event()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var id = await _world.CreateEventAsync();

            var image = await admin.GetAsync($"/api/Events/{id}/image");
            var pdf = await admin.GetAsync($"/api/Events/{id}/pdf-hallview");

            image.StatusCode.ShouldBe(HttpStatusCode.OK);
            image.Content.Headers.ContentType!.MediaType.ShouldBe("image/jpeg");
            pdf.StatusCode.ShouldBe(HttpStatusCode.OK);
            pdf.Content.Headers.ContentType!.MediaType.ShouldBe("application/pdf");
        }

        [Fact]
        public async Task Seeded_event_image_is_served_but_its_hall_view_pdf_is_missing()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var image = await admin.GetAsync("/api/Events/1/image");
            var pdf = await admin.GetAsync("/api/Events/1/pdf-hallview");

            image.StatusCode.ShouldBe(HttpStatusCode.OK);
            pdf.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
