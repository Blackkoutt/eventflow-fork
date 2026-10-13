using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class GeneratedFileTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public GeneratedFileTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private async Task<int> ReserveAsync(RegisteredUser user)
        {
            var eventId = await _world.CreateEventAsync();
            return await _world.ReserveSeatAsync(user.Client, eventId);
        }

        private async Task AssertFileAsync(ApiClient client, string url, string mediaType)
        {
            var response = await client.GetAsync(url);
            response.StatusCode.ShouldBe(HttpStatusCode.OK, await client.ReadErrorsAsync(response));
            response.Content.Headers.ContentType!.MediaType.ShouldBe(mediaType);
            (await response.Content.ReadAsByteArrayAsync()).Length.ShouldBeGreaterThan(500);
        }

        [Fact]
        public async Task Reservation_ticket_is_served_as_pdf_and_jpg_archive_to_the_owner_only()
        {
            var owner = await _world.RegisterUserAsync();
            var stranger = await _world.RegisterUserAsync();
            var reservationId = await ReserveAsync(owner);

            await AssertFileAsync(owner.Client, $"/api/Reservations/{reservationId}/pdf-ticket", "application/pdf");
            var archive = await owner.Client.GetAsync($"/api/Reservations/{reservationId}/jpg-tickets");
            archive.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await stranger.Client.GetAsync($"/api/Reservations/{reservationId}/pdf-ticket")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            (await stranger.Client.GetAsync($"/api/Reservations/{reservationId}/jpg-tickets")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Ticket_of_a_canceled_reservation_is_refused()
        {
            var owner = await _world.RegisterUserAsync();
            var reservationId = await ReserveAsync(owner);
            await owner.Client.DeleteAsync($"/api/Reservations/{reservationId}");

            var response = await owner.Client.GetAsync($"/api/Reservations/{reservationId}/pdf-ticket");

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Event_pass_is_served_as_pdf_and_jpg_to_the_owner_only()
        {
            var owner = await _world.RegisterUserAsync();
            var stranger = await _world.RegisterUserAsync();
            var passId = await _world.BuyPassAsync(owner.Client);

            await AssertFileAsync(owner.Client, $"/api/EventPasses/{passId}/pdf", "application/pdf");
            await AssertFileAsync(owner.Client, $"/api/EventPasses/{passId}/jpg", "image/jpeg");
            (await stranger.Client.GetAsync($"/api/EventPasses/{passId}/pdf")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Hall_rent_invoice_and_hall_view_are_served_to_the_owner_only()
        {
            var owner = await _world.RegisterUserAsync();
            var stranger = await _world.RegisterUserAsync();
            var rentId = await _world.RentHallAsync(owner.Client, services: new[] { 1 });

            await AssertFileAsync(owner.Client, $"/api/HallRents/{rentId}/pdf-invoice", "application/pdf");
            await AssertFileAsync(owner.Client, $"/api/HallRents/{rentId}/pdf-hallview", "application/pdf");
            (await stranger.Client.GetAsync($"/api/HallRents/{rentId}/pdf-invoice")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Files_of_seeded_records_are_currently_missing()
        {
            var owner = await _factory.CreateClientAsync(TestUsers.User3);
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            (await owner.GetAsync("/api/HallRents/2/pdf-invoice")).StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await owner.GetAsync("/api/HallRents/2/pdf-hallview")).StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.GetAsync("/api/Halls/1/pdf-hallview")).StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Hall_view_pdf_is_served_for_a_new_hall()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var hallId = await _world.CreateHallAsync();

            await AssertFileAsync(admin, $"/api/Halls/{hallId}/pdf-hallview", "application/pdf");
        }
    }
}
