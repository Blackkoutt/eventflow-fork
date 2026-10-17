using System.Net;
using System.Text.Json;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class ReservationTests : IClassFixture<ApiFactory>
    {
        private const int SeededFestivalId = 1;

        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public ReservationTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private async Task<(int TicketId, List<int> SeatIds)> PrepareEventAsync(ApiClient client, bool soon = false)
        {
            var eventId = await _world.CreateEventAsync(soon: soon);
            var lookups = new ApiLookups(client);
            return (await lookups.GetNormalTicketIdAsync(eventId), await lookups.GetSeatIdsAsync(eventId));
        }

        [Fact]
        public async Task Reserving_a_free_seat_creates_an_active_reservation_visible_to_the_owner()
        {
            var user = await _world.RegisterUserAsync();
            var (ticketId, seats) = await PrepareEventAsync(user.Client);

            var response = await _world.ReserveAsync(user.Client, ticketId, new[] { seats[0] });

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var created = await user.Client.ReadJsonAsync(response);
            created.GetArrayLength().ShouldBe(1);
            var detail = await user.Client.GetJsonAsync($"/api/Reservations/{created[0].GetProperty("id").GetInt32()}");
            detail.GetProperty("reservationStatus").GetString().ShouldBe("Active");
            detail.GetProperty("user").GetProperty("id").GetString().ShouldBe(user.Id);
            detail.GetProperty("seats").GetArrayLength().ShouldBe(1);
            detail.GetProperty("ticket").GetProperty("id").GetInt32().ShouldBe(ticketId);
            var list = await user.Client.GetJsonAsync("/api/Reservations");
            list.GetArrayLength().ShouldBe(1);
        }

        [Fact]
        public async Task Payment_amount_is_ticket_price_with_vip_seat_surcharge()
        {
            var user = await _world.RegisterUserAsync();
            var (ticketId, seats) = await PrepareEventAsync(user.Client);

            var single = await _world.ReserveAsync(user.Client, ticketId, new[] { seats[0] });
            var pair = await _world.ReserveAsync(user.Client, ticketId, new[] { seats[1], seats[2] });

            (await user.Client.ReadJsonAsync(single))[0].GetProperty("paymentAmount").GetDecimal().ShouldBe(37.50m);
            (await user.Client.ReadJsonAsync(pair))[0].GetProperty("paymentAmount").GetDecimal().ShouldBe(75.00m);
        }

        [Fact]
        public async Task Reserving_a_taken_seat_returns_400()
        {
            var first = await _world.RegisterUserAsync();
            var second = await _world.RegisterUserAsync();
            var (ticketId, seats) = await PrepareEventAsync(first.Client);
            (await _world.ReserveAsync(first.Client, ticketId, new[] { seats[0] })).StatusCode.ShouldBe(HttpStatusCode.OK);

            var response = await _world.ReserveAsync(second.Client, ticketId, new[] { seats[0] });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await second.Client.ReadErrorsAsync(response)).ShouldBe("Jedno z wybranych miejsc jest niedostępne, ponieważ ma rezerwację.");
        }

        [Fact]
        public async Task A_failed_reservation_of_several_seats_does_not_take_the_free_ones()
        {
            var first = await _world.RegisterUserAsync();
            var second = await _world.RegisterUserAsync();
            var (ticketId, seats) = await PrepareEventAsync(first.Client);
            (await _world.ReserveAsync(first.Client, ticketId, new[] { seats[0] })).StatusCode.ShouldBe(HttpStatusCode.OK);

            var failed = await _world.ReserveAsync(second.Client, ticketId, new[] { seats[1], seats[0] });
            var retry = await _world.ReserveAsync(second.Client, ticketId, new[] { seats[1] });

            failed.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            retry.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Reserving_several_seats_at_once_creates_one_reservation_holding_all_seats()
        {
            var user = await _world.RegisterUserAsync();
            var (ticketId, seats) = await PrepareEventAsync(user.Client);

            var response = await _world.ReserveAsync(user.Client, ticketId, new[] { seats[0], seats[1] });

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var created = await user.Client.ReadJsonAsync(response);
            created.GetArrayLength().ShouldBe(1);
            var detail = await user.Client.GetJsonAsync($"/api/Reservations/{created[0].GetProperty("id").GetInt32()}");
            detail.GetProperty("seats").GetArrayLength().ShouldBe(2);
        }

        [Fact]
        public async Task Reservation_without_seats_is_currently_accepted()
        {
            var user = await _world.RegisterUserAsync();
            var (ticketId, _) = await PrepareEventAsync(user.Client);

            var response = await _world.ReserveAsync(user.Client, ticketId, Array.Empty<int>());

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Invalid_reservations_return_400()
        {
            var user = await _world.RegisterUserAsync();
            var (ticketId, seats) = await PrepareEventAsync(user.Client);
            var invalid = new (string Reason, HttpResponseMessage Response, string? Message)[]
            {
                ("duplicated seat ids", await _world.ReserveAsync(user.Client, ticketId, new[] { seats[2], seats[2] }), null),
                ("event ticket with festival flag", await _world.ReserveAsync(user.Client, ticketId, new[] { seats[3] }, forFestival: true), null),
                ("unknown ticket", await _world.ReserveAsync(user.Client, 999, new[] { 1 }), "Bilet o podanym ID nie istnieje w bazie danych."),
                ("unknown payment type", await _world.ReserveAsync(user.Client, ticketId, new[] { seats[3] }, paymentTypeId: 999), "Typ płatności o podanym ID nie istnieje w bazie danych."),
            };
            var failures = new List<string>();

            foreach (var (reason, response, message) in invalid)
            {
                if (response.StatusCode != HttpStatusCode.BadRequest || (message != null && await user.Client.ReadErrorsAsync(response) != message))
                {
                    failures.Add($"{reason}: {(int)response.StatusCode}");
                }
            }

            failures.ShouldBeEmpty();
        }

        [Fact]
        public async Task Paying_with_a_pass_requires_an_active_pass()
        {
            var user = await _world.RegisterUserAsync();
            var (ticketId, seats) = await PrepareEventAsync(user.Client);

            var response = await _world.ReserveAsync(user.Client, ticketId, new[] { seats[3] }, ScenarioWorld.PassPaymentId);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await user.Client.ReadErrorsAsync(response)).ShouldBe("Użytkownik nie posiada aktywnego karnetu.");
        }

        [Fact]
        public async Task A_pass_allows_only_one_reservation_per_event()
        {
            var holder = await _world.RegisterUserAsync();
            await _world.BuyPassAsync(holder.Client, 2);
            var (ticketId, seats) = await PrepareEventAsync(holder.Client, soon: true);

            var first = await _world.ReserveAsync(holder.Client, ticketId, new[] { seats[2] }, ScenarioWorld.PassPaymentId);
            var second = await _world.ReserveAsync(holder.Client, ticketId, new[] { seats[3] }, ScenarioWorld.PassPaymentId);

            first.StatusCode.ShouldBe(HttpStatusCode.OK);
            second.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var reservation = (await holder.Client.ReadJsonAsync(first))[0];
            reservation.GetProperty("paymentAmount").GetDecimal().ShouldBe(0m);
            reservation.GetProperty("paymentType").GetProperty("id").GetInt32().ShouldBe(ScenarioWorld.PassPaymentId);
        }

        [Fact]
        public async Task A_festival_reservation_creates_one_reservation_per_event_without_seat_surcharge()
        {
            var user = await _world.RegisterUserAsync();
            var (festivalId, eventIds) = await _world.CreateFestivalAsync();
            var lookups = new ApiLookups(user.Client);
            var ticketId = await lookups.GetFestivalTicketIdAsync(festivalId);
            var firstEventSeats = await lookups.GetSeatIdsAsync(eventIds[0]);
            var secondEventSeats = await lookups.GetSeatIdsAsync(eventIds[1]);

            var response = await _world.ReserveAsync(user.Client, ticketId, new[] { firstEventSeats[0], secondEventSeats[0] }, forFestival: true);

            response.StatusCode.ShouldBe(HttpStatusCode.OK, await user.Client.ReadErrorsAsync(response));
            var created = await user.Client.ReadJsonAsync(response);
            created.GetArrayLength().ShouldBe(2);
            foreach (var reservation in created.EnumerateArray())
            {
                reservation.GetProperty("paymentAmount").GetDecimal().ShouldBe(50.00m);
            }
        }

        [Fact]
        public async Task Reading_another_users_reservation_returns_403_and_an_unknown_one_404()
        {
            var user = await _world.RegisterUserAsync();

            (await user.Client.GetAsync("/api/Reservations/2")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            (await user.Client.GetAsync("/api/Reservations/999")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Cancelling_own_reservation_marks_it_canceled_and_frees_the_seats()
        {
            var owner = await _world.RegisterUserAsync();
            var other = await _world.RegisterUserAsync();
            var (ticketId, seats) = await PrepareEventAsync(owner.Client);
            var created = await owner.Client.ReadJsonAsync(await _world.ReserveAsync(owner.Client, ticketId, new[] { seats[0] }));
            var reservationId = created[0].GetProperty("id").GetInt32();

            var cancel = await owner.Client.DeleteAsync($"/api/Reservations/{reservationId}");
            var detail = await owner.Client.GetJsonAsync($"/api/Reservations/{reservationId}");
            var again = await owner.Client.DeleteAsync($"/api/Reservations/{reservationId}");
            var reuse = await _world.ReserveAsync(other.Client, ticketId, new[] { seats[0] });

            cancel.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            detail.GetProperty("reservationStatus").GetString().ShouldBe("Canceled");
            detail.GetProperty("deleteDate").ValueKind.ShouldNotBe(JsonValueKind.Null);
            again.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await owner.Client.ReadErrorsAsync(again)).ShouldBe("Rezerwacja została anulowana.");
            reuse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Only_the_owner_or_an_admin_can_cancel_and_an_unknown_reservation_returns_400()
        {
            var user = await _world.RegisterUserAsync();
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            (await user.Client.DeleteAsync("/api/Reservations/5")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            (await admin.DeleteAsync("/api/Reservations/3")).StatusCode.ShouldBe(HttpStatusCode.NoContent);
            (await admin.DeleteAsync("/api/Reservations/999")).StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
