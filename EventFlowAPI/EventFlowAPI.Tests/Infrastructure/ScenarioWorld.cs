using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace EventFlowAPI.Tests.Infrastructure
{
    public record RegisteredUser(ApiClient Client, string Id, string Email);

    public class ScenarioWorld
    {
        public const int PayUPaymentId = 1;
        public const int PassPaymentId = 2;

        private const int FirstFreeDay = 1000;
        private const string Password = "Haslo123!";

        private static readonly (int Price, int TicketTypeId)[] DefaultEventTickets = { (30, 1), (20, 2) };

        private readonly ApiFactory _factory;

        public ScenarioWorld(ApiFactory factory)
        {
            _factory = factory;
        }

        public DateTime SlotStart(int slot, int hour = 10)
        {
            return DateTime.Today.AddDays(FirstFreeDay + slot).AddHours(hour);
        }

        public DateTime NextFarStart()
        {
            return SlotStart(_factory.NextSlot());
        }

        public DateTime NextSoonStart()
        {
            var slot = _factory.NextSlot();
            return DateTime.Today.AddDays(3 + slot / 8).AddHours(8 + 2 * (slot % 8));
        }

        public string UniqueName(string prefix)
        {
            return $"{prefix} {Guid.NewGuid().ToString("N")[..8]}";
        }

        public FormData EventForm(string name, int hallId, DateTime start, DateTime end, params (int Price, int TicketTypeId)[] tickets)
        {
            var form = new FormData()
                .Add("Name", name)
                .Add("ShortDescription", "krotki opis")
                .Add("LongDescription", "dlugi opis")
                .Add("CategoryId", 1)
                .Add("HallId", hallId)
                .Add("StartDate", start.ToString("s"))
                .Add("EndDate", end.ToString("s"));
            var index = 0;
            foreach (var ticket in tickets)
            {
                form.Add($"EventTickets[{index}].Price", ticket.Price).Add($"EventTickets[{index}].TicketTypeId", ticket.TicketTypeId);
                index++;
            }
            return form;
        }

        public const int SeededOrganizerIdOffset = 3;
        public const int SeededSponsorIdOffset = 6;

        public FormData FestivalForm(string name, IEnumerable<int> eventIds, int partnerId, params (int Price, int TicketTypeId)[] tickets)
        {
            var form = new FormData()
                .Add("Name", name)
                .Add("ShortDescription", "krotki opis")
                .Add("LongDescription", "dlugi opis")
                .Add("MediaPatronIds", partnerId)
                .Add("OrganizerIds", partnerId + SeededOrganizerIdOffset)
                .Add("SponsorIds", partnerId + SeededSponsorIdOffset);
            var index = 0;
            foreach (var ticket in tickets)
            {
                form.Add($"FestivalTickets[{index}].Price", ticket.Price).Add($"FestivalTickets[{index}].TicketTypeId", ticket.TicketTypeId);
                index++;
            }
            foreach (var eventId in eventIds)
            {
                form.Add("EventIds", eventId);
            }
            return form;
        }

        public async Task<RegisteredUser> RegisterUserAsync()
        {
            var email = $"user{Guid.NewGuid():N}@example.com";
            var anonymous = _factory.CreateClient();
            var registration = new
            {
                name = "Jan",
                surname = "Testowy",
                email,
                dateOfBirth = "1990-01-01",
                password = Password,
                confirmPassword = Password,
            };
            var register = await anonymous.PostAsJsonAsync("/api/Auth/register", registration);
            register.EnsureSuccessStatusCode();

            var httpClient = _factory.CreateClient();
            var credentials = new { email, password = Password };
            var login = await httpClient.PostAsJsonAsync("/api/Auth/login", credentials);
            login.EnsureSuccessStatusCode();
            var token = JsonDocument.Parse(await login.Content.ReadAsStringAsync()).RootElement.GetProperty("token").GetString();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var client = new ApiClient(httpClient);
            var info = await client.GetJsonAsync("/api/Users/info");
            return new RegisteredUser(client, info.GetProperty("id").GetString()!, email);
        }

        public async Task<int> CreateEventAsync(int hallId = 1, bool soon = false)
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var start = soon ? NextSoonStart() : NextFarStart();
            var form = EventForm(UniqueName("Wydarzenie"), hallId, start, start.AddHours(1), DefaultEventTickets);
            var response = await admin.PostFormAsync("/api/Events", form);
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await admin.ReadErrorsAsync(response));
            return (await admin.ReadJsonAsync(response)).GetProperty("id").GetInt32();
        }

        public async Task<(int FestivalId, List<int> EventIds)> CreateFestivalAsync()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var eventIds = new List<int>
            {
                await CreateEventAsync(soon: true),
                await CreateEventAsync(soon: true),
            };
            var form = FestivalForm(UniqueName("Festiwal"), eventIds, 1, (50, ApiLookups.NormalTicketTypeId));
            var response = await admin.PostFormAsync("/api/Festivals", form);
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await admin.ReadErrorsAsync(response));
            return ((await admin.ReadJsonAsync(response)).GetProperty("id").GetInt32(), eventIds);
        }

        public List<object> SeatRow(int seatCount, int seatTypeId = 3)
        {
            var seats = new List<object>();
            foreach (var number in Enumerable.Range(1, seatCount))
            {
                seats.Add(new { seatNr = number, row = 1, gridRow = 1, column = number, gridColumn = number, seatTypeId });
            }
            return seats;
        }

        public async Task<int> CreateHallAsync(int seatCount = 4)
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var seats = SeatRow(seatCount);
            var hall = new
            {
                hallNr = 500 + _factory.NextSlot(),
                rentalPricePerHour = 100,
                floor = 1,
                hallDetails = new { totalLength = 20.0, totalWidth = 10.0, stageLength = 5.0, stageWidth = 8.0, maxNumberOfSeatsRows = 5, maxNumberOfSeatsColumns = 10 },
                hallTypeId = 1,
                seats,
            };
            var response = await admin.PostAsync("/api/Halls", hall);
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await admin.ReadErrorsAsync(response));
            return (await admin.ReadJsonAsync(response)).GetProperty("id").GetInt32();
        }

        public async Task<HttpResponseMessage> ReserveAsync(ApiClient client, int ticketId, IEnumerable<int> seatIds, int paymentTypeId = PayUPaymentId, bool forFestival = false)
        {
            var reservation = new
            {
                paymentTypeId,
                ticketId,
                isReservationForFestival = forFestival,
                seatsIds = seatIds.ToArray(),
            };
            return await client.PostAsync("/api/Reservations", reservation);
        }

        public async Task<int> ReserveSeatAsync(ApiClient client, int eventId, int seatIndex = 0, int paymentTypeId = PayUPaymentId)
        {
            var lookups = new ApiLookups(client);
            var ticketId = await lookups.GetNormalTicketIdAsync(eventId);
            var seats = await lookups.GetSeatIdsAsync(eventId);
            var response = await ReserveAsync(client, ticketId, new[] { seats[seatIndex] }, paymentTypeId);
            response.StatusCode.ShouldBe(HttpStatusCode.OK, await client.ReadErrorsAsync(response));
            return (await client.ReadJsonAsync(response))[0].GetProperty("id").GetInt32();
        }

        public async Task<HttpResponseMessage> RentAsync(ApiClient client, DateTime start, int hours = 2, int hallId = 2, int[]? services = null)
        {
            var rent = new
            {
                startDate = start.ToString("s"),
                endDate = start.AddHours(hours).ToString("s"),
                hallId,
                additionalServicesIds = services ?? Array.Empty<int>(),
            };
            return await client.PostAsync("/api/HallRents", rent);
        }

        public async Task<int> RentHallAsync(ApiClient client, int hallId = 2, int[]? services = null)
        {
            var response = await RentAsync(client, NextFarStart(), hallId: hallId, services: services);
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await client.ReadErrorsAsync(response));
            return (await client.ReadJsonAsync(response)).GetProperty("id").GetInt32();
        }

        public async Task<int> BuyPassAsync(ApiClient client, int passTypeId = 1)
        {
            var pass = new { passTypeId };
            var response = await client.PostAsync("/api/EventPasses", pass);
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await client.ReadErrorsAsync(response));
            return (await client.ReadJsonAsync(response)).GetProperty("id").GetInt32();
        }
    }
}
