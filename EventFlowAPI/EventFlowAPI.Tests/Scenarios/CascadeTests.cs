using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class CascadeTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public CascadeTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private FormData MoveEventForm(string name, int hallId, DateTime start)
        {
            return _world.EventForm(name, hallId, start, start.AddHours(1), (30, ApiLookups.NormalTicketTypeId), (20, ApiLookups.ReducedTicketTypeId));
        }

        [Fact]
        public async Task Deleting_an_event_of_a_two_event_festival_cancels_the_festival()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var lookups = new ApiLookups(admin);
            var (festivalId, eventIds) = await _world.CreateFestivalAsync();

            var response = await admin.DeleteAsync($"/api/Events/{eventIds[0]}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            (await lookups.GetStatusAsync($"/api/Events/{eventIds[0]}", "eventStatus")).ShouldBe("Canceled");
            (await lookups.GetStatusAsync($"/api/Festivals/{festivalId}", "festivalStatus")).ShouldBe("Canceled");
            (await lookups.GetStatusAsync($"/api/Events/{eventIds[1]}", "eventStatus")).ShouldBe("Active");
        }

        [Fact]
        public async Task Moving_an_event_in_time_moves_its_reservations_but_currently_not_the_festival_dates()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var (festivalId, eventIds) = await _world.CreateFestivalAsync();
            var reservationId = await _world.ReserveSeatAsync(user.Client, eventIds[0]);
            var original = await admin.GetJsonAsync($"/api/Events/{eventIds[0]}");
            var newStart = original.GetProperty("start").GetDateTime().AddHours(-3);

            var response = await admin.PutFormAsync($"/api/Events/{eventIds[0]}", MoveEventForm(original.GetProperty("title").GetString()!, 1, newStart));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await admin.ReadErrorsAsync(response));
            var reservation = await user.Client.GetJsonAsync($"/api/Reservations/{reservationId}");
            reservation.GetProperty("reservationStatus").GetString().ShouldBe("Active");
            reservation.GetProperty("ticket").GetProperty("event").GetProperty("start").GetDateTime().ShouldBe(newStart);
            var festival = await admin.GetJsonAsync($"/api/Festivals/{festivalId}");
            festival.GetProperty("start").GetDateTime().ShouldBe(original.GetProperty("start").GetDateTime());
        }

        [Fact]
        public async Task Moving_an_event_to_another_hall_moves_its_reservations_to_seats_of_the_new_hall()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var lookups = new ApiLookups(admin);
            var eventId = await _world.CreateEventAsync(hallId: 1);
            var reservationId = await _world.ReserveSeatAsync(user.Client, eventId);
            var original = await admin.GetJsonAsync($"/api/Events/{eventId}");
            var start = original.GetProperty("start").GetDateTime();

            var response = await admin.PutFormAsync($"/api/Events/{eventId}", MoveEventForm(original.GetProperty("title").GetString()!, 3, start));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await admin.ReadErrorsAsync(response));
            var moved = await admin.GetJsonAsync($"/api/Events/{eventId}");
            moved.GetProperty("hall").GetProperty("hallNr").GetInt32().ShouldBe(3);
            var newHallSeats = await lookups.GetSeatIdsAsync(eventId);
            var reservation = await user.Client.GetJsonAsync($"/api/Reservations/{reservationId}");
            reservation.GetProperty("reservationStatus").GetString().ShouldBe("Active");
            var reservedSeats = lookups.ReadIds(reservation.GetProperty("seats"));
            reservedSeats.Count.ShouldBe(1);
            newHallSeats.ShouldContain(reservedSeats[0]);
        }

        [Fact]
        public async Task Moving_an_event_so_that_its_festival_exceeds_14_days_is_rejected_after_the_event_was_already_saved()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var (festivalId, eventIds) = await _world.CreateFestivalAsync();
            var original = await admin.GetJsonAsync($"/api/Events/{eventIds[1]}");
            var farStart = original.GetProperty("start").GetDateTime().AddDays(20);

            var response = await admin.PutFormAsync($"/api/Events/{eventIds[1]}", MoveEventForm(original.GetProperty("title").GetString()!, 1, farStart));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.GetJsonAsync($"/api/Events/{eventIds[1]}")).GetProperty("start").GetDateTime().ShouldBe(farStart);
            (await admin.GetJsonAsync($"/api/Festivals/{festivalId}")).GetProperty("end").GetDateTime().ShouldBeLessThan(farStart);
        }

        [Fact]
        public async Task Cancelling_a_pass_cancels_reservations_paid_with_it()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var passId = await _world.BuyPassAsync(user.Client);
            var eventId = await _world.CreateEventAsync(soon: true);
            var reservationId = await _world.ReserveSeatAsync(user.Client, eventId, paymentTypeId: ScenarioWorld.PassPaymentId);

            var response = await admin.DeleteAsync($"/api/EventPasses/{passId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            (await new ApiLookups(user.Client).GetStatusAsync($"/api/Reservations/{reservationId}", "reservationStatus")).ShouldBe("Canceled");
        }

        [Fact]
        public async Task Changing_a_hall_number_renumbers_the_copies_used_by_events_and_rents()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var hallId = await _world.CreateHallAsync();
            var eventId = await _world.CreateEventAsync(hallId);
            var reservationId = await _world.ReserveSeatAsync(user.Client, eventId);
            var rentId = await _world.RentHallAsync(user.Client, hallId);
            var newNumber = 900 + _factory.NextSlot();
            var update = new
            {
                hallNr = newNumber,
                rentalPricePerHour = 100,
                floor = 1,
                hallTypeId = 1,
                seats = _world.SeatRow(4),
            };

            var response = await admin.PutAsync($"/api/Halls/{hallId}", update);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await admin.ReadErrorsAsync(response));
            (await admin.GetJsonAsync($"/api/Events/{eventId}")).GetProperty("hall").GetProperty("hallNr").GetInt32().ShouldBe(newNumber);
            (await user.Client.GetJsonAsync($"/api/HallRents/{rentId}")).GetProperty("hall").GetProperty("hallNr").GetInt32().ShouldBe(newNumber);
            (await new ApiLookups(user.Client).GetStatusAsync($"/api/Reservations/{reservationId}", "reservationStatus")).ShouldBe("Active");
        }

        [Fact]
        public async Task Deleting_a_hall_cancels_its_events_rents_and_reservations()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var lookups = new ApiLookups(user.Client);
            var hallId = await _world.CreateHallAsync();
            var eventId = await _world.CreateEventAsync(hallId);
            var reservationId = await _world.ReserveSeatAsync(user.Client, eventId);
            var rentId = await _world.RentHallAsync(user.Client, hallId);

            var response = await admin.DeleteAsync($"/api/Halls/{hallId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent, await admin.ReadErrorsAsync(response));
            (await admin.GetAsync($"/api/Halls/{hallId}")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
            (await lookups.GetStatusAsync($"/api/Events/{eventId}", "eventStatus")).ShouldBe("Canceled");
            (await lookups.GetStatusAsync($"/api/Reservations/{reservationId}", "reservationStatus")).ShouldBe("Canceled");
            (await lookups.GetStatusAsync($"/api/HallRents/{rentId}", "hallRentStatus")).ShouldBe("Canceled");
        }
    }
}
