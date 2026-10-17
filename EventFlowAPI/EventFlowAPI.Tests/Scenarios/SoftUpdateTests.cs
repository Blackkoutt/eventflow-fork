using System.Text.Json;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class SoftUpdateTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public SoftUpdateTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private static Dictionary<int, string?> NamesById(JsonElement list)
        {
            return list.EnumerateArray().ToDictionary(item => item.GetProperty("id").GetInt32(), item => item.GetProperty("name").GetString());
        }

        [Fact]
        public async Task Ticket_type_used_by_reservations_is_currently_updated_in_place()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var lookups = new ApiLookups(user.Client);
            var start = _world.NextFarStart();
            var eventForm = _world.EventForm(_world.UniqueName("Wydarzenie"), 2, start, start.AddHours(1), (30, ApiLookups.NormalTicketTypeId), (15, 3));
            var eventId = (await admin.ReadJsonAsync(await admin.PostFormAsync("/api/Events", eventForm))).GetProperty("id").GetInt32();
            var ticketId = await lookups.GetTicketIdAsync($"/api/Events/{eventId}", 3);
            var seats = await lookups.GetSeatIdsAsync(eventId);
            var reservation = await user.Client.ReadJsonAsync(await _world.ReserveAsync(user.Client, ticketId, new[] { seats[0] }));
            var reservationId = reservation[0].GetProperty("id").GetInt32();
            var update = new { name = "Bilet zmieniony" };

            var response = await admin.PutAsync("/api/TicketTypes/3", update);

            var reserved = await user.Client.GetJsonAsync($"/api/Reservations/{reservationId}");
            var eventTickets = await admin.GetJsonAsync($"/api/Events/{eventId}");
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
            var ticketTypes = NamesById(await admin.GetJsonAsync("/api/TicketTypes"));
            ticketTypes[1].ShouldBe("Bilet normalny");
            ticketTypes[2].ShouldBe("Bilet ulgowy");
            ticketTypes[3].ShouldBe("Bilet zmieniony");
            reserved.GetProperty("ticket").GetProperty("ticketType").GetProperty("name").GetString().ShouldBe("Bilet zmieniony");
            var eventTicketTypes = eventTickets.GetProperty("tickets").EnumerateArray()
                .ToDictionary(ticket => ticket.GetProperty("ticketType").GetProperty("id").GetInt32(), ticket => ticket.GetProperty("ticketType").GetProperty("name").GetString());
            eventTicketTypes.ShouldBe(new Dictionary<int, string?> { [1] = "Bilet normalny", [3] = "Bilet zmieniony" }, ignoreOrder: true);
        }

        [Fact]
        public async Task Seat_type_used_by_reservations_is_currently_updated_in_place()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var eventId = await _world.CreateEventAsync(hallId: 1);
            var reservationId = await _world.ReserveSeatAsync(user.Client, eventId);
            var update = new { name = "Miejsce VIP zmienione", description = "nowy opis", seatColor = "#123456", addtionalPaymentPercentage = 30.0 };

            var response = await admin.PutAsync("/api/SeatTypes/1", update);

            var reservation = await user.Client.GetJsonAsync($"/api/Reservations/{reservationId}");
            var eventJson = await admin.GetJsonAsync($"/api/Events/{eventId}");
            var hall = await admin.GetJsonAsync($"/api/Halls/{eventJson.GetProperty("hall").GetProperty("id").GetInt32()}");
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
            var seatTypes = NamesById(await admin.GetJsonAsync("/api/SeatTypes"));
            seatTypes[1].ShouldBe("Miejsce VIP zmienione");
            seatTypes[2].ShouldBe("Miejsce klasy premium");
            seatTypes[3].ShouldBe("Miejsce zwykłe");
            reservation.GetProperty("seats")[0].GetProperty("seatType").GetProperty("name").GetString().ShouldBe("Miejsce VIP zmienione");
            hall.GetProperty("seats").EnumerateArray().ShouldAllBe(seat => seat.GetProperty("seatType").GetProperty("name").GetString() == "Miejsce VIP zmienione");
        }

        [Fact]
        public async Task Hall_type_used_by_rented_halls_is_currently_updated_in_place()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var user = await _world.RegisterUserAsync();
            var rentId = await _world.RentHallAsync(user.Client, hallId: 1);
            var form = new FormData().Add("Name", "Sala filmowa zmieniona").Add("Description", "nowy opis").Add("EquipmentIds", 1);

            var response = await admin.PutFormAsync("/api/HallTypes/1", form);

            var rent = await user.Client.GetJsonAsync($"/api/HallRents/{rentId}");
            var rentedHall = await admin.GetJsonAsync($"/api/Halls/{rent.GetProperty("hall").GetProperty("id").GetInt32()}");
            var baseHall = await admin.GetJsonAsync("/api/Halls/1");
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
            var hallTypes = NamesById(await admin.GetJsonAsync("/api/HallTypes"));
            hallTypes.Count.ShouldBe(6);
            hallTypes[1].ShouldBe("Sala filmowa zmieniona");
            hallTypes[2].ShouldBe("Sala koncertowa");
            rentedHall.GetProperty("type").GetProperty("name").GetString().ShouldBe("Sala filmowa zmieniona");
            baseHall.GetProperty("type").GetProperty("name").GetString().ShouldBe("Sala filmowa zmieniona");
        }
    }
}
