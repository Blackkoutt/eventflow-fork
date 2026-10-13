using System.Text;
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

        private string DescribeList(JsonElement list, string nameProperty)
        {
            var lines = new StringBuilder();
            foreach (var item in list.EnumerateArray())
            {
                lines.AppendLine($"{item.GetProperty("id").GetInt32()}: {item.GetProperty(nameProperty).GetString()}");
            }
            return lines.ToString();
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
            var snapshot = new StringBuilder()
                .AppendLine($"status: {(int)response.StatusCode}")
                .AppendLine("ticket types:")
                .Append(DescribeList(await admin.GetJsonAsync("/api/TicketTypes"), "name"))
                .AppendLine($"reserved ticket type: {reserved.GetProperty("ticket").GetProperty("ticketType").GetProperty("name").GetString()}")
                .AppendLine("event ticket types:");
            foreach (var ticket in eventTickets.GetProperty("tickets").EnumerateArray())
            {
                snapshot.AppendLine($"{ticket.GetProperty("ticketType").GetProperty("id").GetInt32()}: {ticket.GetProperty("ticketType").GetProperty("name").GetString()}");
            }

            await Verifier.Verify(snapshot.ToString()).UseDirectory("SoftUpdateSnapshots");
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
            var snapshot = new StringBuilder()
                .AppendLine($"status: {(int)response.StatusCode}")
                .AppendLine("seat types:")
                .Append(DescribeList(await admin.GetJsonAsync("/api/SeatTypes"), "name"))
                .AppendLine($"reserved seat type: {reservation.GetProperty("seats")[0].GetProperty("seatType").GetProperty("name").GetString()}")
                .AppendLine("event hall seats:");
            foreach (var seat in hall.GetProperty("seats").EnumerateArray())
            {
                snapshot.AppendLine($"{seat.GetProperty("seatNr").GetInt32()}: {seat.GetProperty("seatType").GetProperty("name").GetString()}");
            }

            await Verifier.Verify(snapshot.ToString()).UseDirectory("SoftUpdateSnapshots");
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
            var snapshot = new StringBuilder()
                .AppendLine($"status: {(int)response.StatusCode}")
                .AppendLine("hall types:")
                .Append(DescribeList(await admin.GetJsonAsync("/api/HallTypes"), "name"))
                .AppendLine($"rented hall type: {rentedHall.GetProperty("type").GetProperty("name").GetString()}")
                .AppendLine($"base hall type: {baseHall.GetProperty("type").GetProperty("name").GetString()}");

            await Verifier.Verify(snapshot.ToString()).UseDirectory("SoftUpdateSnapshots");
        }
    }
}
