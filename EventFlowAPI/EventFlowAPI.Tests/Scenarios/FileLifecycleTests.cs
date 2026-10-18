using System.IO.Compression;
using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class FileLifecycleTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public FileLifecycleTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private async Task AssertServedAsync(ApiClient client, string url, string mediaType)
        {
            var response = await client.GetAsync(url);
            response.StatusCode.ShouldBe(HttpStatusCode.OK, url + " " + await client.ReadErrorsAsync(response));
            response.Content.Headers.ContentType!.MediaType.ShouldBe(mediaType, url);
            (await response.Content.ReadAsByteArrayAsync()).Length.ShouldBeGreaterThan(500, url);
        }

        private async Task AssertStatusAsync(ApiClient client, string url, HttpStatusCode expected)
        {
            (await client.GetAsync(url)).StatusCode.ShouldBe(expected, url);
        }

        private async Task<int> CountArchiveEntriesAsync(ApiClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.StatusCode.ShouldBe(HttpStatusCode.OK, url);
            using var archive = new ZipArchive(new MemoryStream(await response.Content.ReadAsByteArrayAsync()));
            return archive.Entries.Count;
        }

        private FormData MoveEventForm(string name, int hallId, DateTime start)
        {
            return _world.EventForm(name, hallId, start, start.AddHours(1), (30, ApiLookups.NormalTicketTypeId), (20, ApiLookups.ReducedTicketTypeId));
        }

        [Theory]
        [InlineData("/api/Reservations/1/pdf-ticket", "application/pdf")]
        [InlineData("/api/Reservations/1/jpg-tickets", "application/zip")]
        [InlineData("/api/EventPasses/1/pdf", "application/pdf")]
        [InlineData("/api/EventPasses/1/jpg", "image/jpeg")]
        [InlineData("/api/HallRents/1/pdf-invoice", "application/pdf")]
        [InlineData("/api/HallRents/1/pdf-hallview", "application/pdf")]
        [InlineData("/api/Halls/1/pdf-hallview", "application/pdf")]
        [InlineData("/api/Halls/2/pdf-hallview", "application/pdf")]
        [InlineData("/api/Events/1/pdf-hallview", "application/pdf")]
        public async Task Files_of_seeded_records_are_generated_on_request(string url, string mediaType)
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            await AssertServedAsync(admin, url, mediaType);
        }

        [Theory]
        [InlineData("/api/Reservations/999999/pdf-ticket")]
        [InlineData("/api/EventPasses/999999/pdf")]
        [InlineData("/api/HallRents/999999/pdf-invoice")]
        [InlineData("/api/Halls/999999/pdf-hallview")]
        public async Task Files_of_unknown_records_return_404(string url)
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            await AssertStatusAsync(admin, url, HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task A_festival_reservation_gets_one_ticket_image_per_event_and_one_pdf()
        {
            var user = await _world.RegisterUserAsync();
            var (festivalId, eventIds) = await _world.CreateFestivalAsync();
            var lookups = new ApiLookups(user.Client);
            var ticketId = await lookups.GetFestivalTicketIdAsync(festivalId);
            var firstSeats = await lookups.GetSeatIdsAsync(eventIds[0]);
            var secondSeats = await lookups.GetSeatIdsAsync(eventIds[1]);
            var response = await _world.ReserveAsync(user.Client, ticketId, new[] { firstSeats[0], secondSeats[0] }, forFestival: true);
            var reservationId = (await user.Client.ReadJsonAsync(response))[0].GetProperty("id").GetInt32();

            await AssertServedAsync(user.Client, $"/api/Reservations/{reservationId}/pdf-ticket", "application/pdf");
            (await CountArchiveEntriesAsync(user.Client, $"/api/Reservations/{reservationId}/jpg-tickets")).ShouldBe(2);
        }

        [Fact]
        public async Task Two_seats_of_one_event_give_one_reservation_with_one_ticket_image()
        {
            var user = await _world.RegisterUserAsync();
            var eventId = await _world.CreateEventAsync();
            var lookups = new ApiLookups(user.Client);
            var seats = await lookups.GetSeatIdsAsync(eventId);
            var response = await _world.ReserveAsync(user.Client, await lookups.GetNormalTicketIdAsync(eventId), new[] { seats[0], seats[1] });
            var created = await user.Client.ReadJsonAsync(response);
            var reservationId = created[0].GetProperty("id").GetInt32();

            created.GetArrayLength().ShouldBe(1);
            await AssertServedAsync(user.Client, $"/api/Reservations/{reservationId}/pdf-ticket", "application/pdf");
            (await CountArchiveEntriesAsync(user.Client, $"/api/Reservations/{reservationId}/jpg-tickets")).ShouldBe(1);
        }

        [Fact]
        public async Task Files_stay_available_after_the_event_moves_to_another_hall()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var eventId = await _world.CreateEventAsync(hallId: 1);
            var reservationId = await _world.ReserveSeatAsync(user.Client, eventId);
            var original = await admin.GetJsonAsync($"/api/Events/{eventId}");

            var response = await admin.PutFormAsync($"/api/Events/{eventId}", MoveEventForm(original.GetProperty("title").GetString()!, 3, original.GetProperty("start").GetDateTime()));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await admin.ReadErrorsAsync(response));
            await AssertServedAsync(user.Client, $"/api/Reservations/{reservationId}/pdf-ticket", "application/pdf");
            await AssertServedAsync(admin, $"/api/Events/{eventId}/pdf-hallview", "application/pdf");
        }

        [Fact]
        public async Task Files_stay_available_after_the_hall_is_edited()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var hallId = await _world.CreateHallAsync();
            var eventId = await _world.CreateEventAsync(hallId);
            var reservationId = await _world.ReserveSeatAsync(user.Client, eventId);
            var rentId = await _world.RentHallAsync(user.Client, hallId);
            var update = new
            {
                hallNr = 900 + _factory.NextSlot(),
                rentalPricePerHour = 100,
                floor = 1,
                hallTypeId = 1,
                seats = _world.SeatRow(4),
            };

            var response = await admin.PutAsync($"/api/Halls/{hallId}", update);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await admin.ReadErrorsAsync(response));
            await AssertServedAsync(admin, $"/api/Halls/{hallId}/pdf-hallview", "application/pdf");
            await AssertServedAsync(admin, $"/api/Events/{eventId}/pdf-hallview", "application/pdf");
            await AssertServedAsync(user.Client, $"/api/HallRents/{rentId}/pdf-hallview", "application/pdf");
            await AssertServedAsync(user.Client, $"/api/HallRents/{rentId}/pdf-invoice", "application/pdf");
            await AssertServedAsync(user.Client, $"/api/Reservations/{reservationId}/pdf-ticket", "application/pdf");
            (await CountArchiveEntriesAsync(user.Client, $"/api/Reservations/{reservationId}/jpg-tickets")).ShouldBe(1);
        }

        [Fact]
        public async Task Pass_files_stay_available_after_a_renewal()
        {
            var user = await _world.RegisterUserAsync();
            var passId = await _world.BuyPassAsync(user.Client);

            var response = await user.Client.PutAsync($"/api/EventPasses/{passId}", new { passTypeId = 2 });

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            await AssertServedAsync(user.Client, $"/api/EventPasses/{passId}/pdf", "application/pdf");
            await AssertServedAsync(user.Client, $"/api/EventPasses/{passId}/jpg", "image/jpeg");
        }

        [Fact]
        public async Task Files_of_a_deleted_rent_are_refused()
        {
            var user = await _world.RegisterUserAsync();
            var rentId = await _world.RentHallAsync(user.Client);
            await user.Client.DeleteAsync($"/api/HallRents/{rentId}");

            await AssertStatusAsync(user.Client, $"/api/HallRents/{rentId}/pdf-invoice", HttpStatusCode.BadRequest);
            await AssertStatusAsync(user.Client, $"/api/HallRents/{rentId}/pdf-hallview", HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Files_of_a_canceled_pass_are_refused()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var passId = await _world.BuyPassAsync(user.Client);
            await admin.DeleteAsync($"/api/EventPasses/{passId}");

            await AssertStatusAsync(user.Client, $"/api/EventPasses/{passId}/pdf", HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Files_of_a_deleted_hall_and_a_deleted_event_are_gone()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var hallId = await _world.CreateHallAsync();
            var eventId = await _world.CreateEventAsync();
            await admin.DeleteAsync($"/api/Halls/{hallId}");
            await admin.DeleteAsync($"/api/Events/{eventId}");

            await AssertStatusAsync(admin, $"/api/Halls/{hallId}/pdf-hallview", HttpStatusCode.NotFound);
            await AssertStatusAsync(admin, $"/api/Events/{eventId}/pdf-hallview", HttpStatusCode.BadRequest);
        }
    }
}
