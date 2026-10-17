using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class HallTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;

        public HallTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private object SeatPayload(int number, int row = 1, int column = 1, int seatTypeId = 3)
        {
            return new { seatNr = number, row, gridRow = row, column, gridColumn = column, seatTypeId };
        }

        private object HallPayload(int hallNr, int hallTypeId = 1, object[]? seats = null, double price = 100.5)
        {
            return new
            {
                hallNr,
                rentalPricePerHour = price,
                floor = 2,
                totalLength = 20.0,
                totalWidth = 10.0,
                stageLength = 5.0,
                stageWidth = 8.0,
                maxNumberOfSeatsRows = 5,
                maxNumberOfSeatsColumns = 5,
                hallTypeId,
                seats = seats ?? new[] { SeatPayload(1), SeatPayload(2, column: 2, seatTypeId: 1) },
            };
        }

        private int NextHallNumber()
        {
            return 100 + _factory.NextSlot();
        }

        private async Task<int> CreateAsync(ApiClient client, object payload)
        {
            var response = await client.PostAsync("/api/Halls", payload);
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await client.ReadErrorsAsync(response));
            return (await client.ReadJsonAsync(response)).GetProperty("id").GetInt32();
        }

        [Fact]
        public async Task Creating_a_hall_stores_details_and_seats()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var hallNr = NextHallNumber();

            var id = await CreateAsync(admin, HallPayload(hallNr));

            var hall = await admin.GetJsonAsync($"/api/Halls/{id}");
            hall.GetProperty("hallNr").GetInt32().ShouldBe(hallNr);
            hall.GetProperty("rentalPricePerHour").GetDecimal().ShouldBe(100.5m);
            hall.GetProperty("seatsCount").GetInt32().ShouldBe(2);
            hall.GetProperty("seats").GetArrayLength().ShouldBe(2);
            hall.GetProperty("totalArea").GetDecimal().ShouldBe(200m);
            hall.GetProperty("type").GetProperty("id").GetInt32().ShouldBe(1);
        }

        [Fact]
        public async Task A_regular_user_cannot_create_a_hall()
        {
            var user = await _factory.CreateClientAsync(TestUsers.User5);

            var response = await user.PostAsync("/api/Halls", HallPayload(NextHallNumber()));

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Invalid_halls_return_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var invalid = new (string Reason, object Payload, string? Message)[]
            {
                ("duplicate seat numbers", HallPayload(NextHallNumber(), seats: new[] { SeatPayload(1), SeatPayload(1) }), null),
                ("seat outside the hall grid", HallPayload(NextHallNumber(), seats: new[] { SeatPayload(1, row: 9) }), null),
                ("unknown hall type", HallPayload(NextHallNumber(), hallTypeId: 999), "Typ sali o podanym ID nie istnieje w bazie danych."),
                ("empty hall", new { hallNr = 0 }, null),
            };
            var failures = new List<string>();

            foreach (var (reason, payload, message) in invalid)
            {
                var response = await admin.PostAsync("/api/Halls", payload);
                if (response.StatusCode != HttpStatusCode.BadRequest || (message != null && await admin.ReadErrorsAsync(response) != message))
                {
                    failures.Add($"{reason}: {(int)response.StatusCode}");
                }
            }

            failures.ShouldBeEmpty();
        }

        [Fact]
        public async Task Updating_a_hall_replaces_price_type_and_seats()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var hallNr = NextHallNumber();
            var id = await CreateAsync(admin, HallPayload(hallNr));

            var response = await admin.PutAsync($"/api/Halls/{id}", HallPayload(hallNr, hallTypeId: 2, seats: new[] { SeatPayload(1) }, price: 110));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var hall = await admin.GetJsonAsync($"/api/Halls/{id}");
            hall.GetProperty("rentalPricePerHour").GetDecimal().ShouldBe(110m);
            hall.GetProperty("seatsCount").GetInt32().ShouldBe(1);
            hall.GetProperty("type").GetProperty("id").GetInt32().ShouldBe(2);
        }

        [Fact]
        public async Task Deleting_a_hall_removes_it()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var id = await CreateAsync(admin, HallPayload(NextHallNumber()));

            var response = await admin.DeleteAsync($"/api/Halls/{id}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            (await admin.GetAsync($"/api/Halls/{id}")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
