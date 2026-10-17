using System.Text.Json;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Queries
{
    [Collection(ReadOnlyApiCollection.Name)]
    public class SeededResponseTests
    {
        private static readonly string StatisticsWindow =
            $"StartDate={DateTime.Today.AddDays(-30):yyyy-MM-dd}&EndDate={DateTime.Today.AddDays(120):yyyy-MM-dd}";

        private static readonly string AllStatisticsSections =
            "IncludeStatisticsAboutHallRent=true&IncludeStatisticsAboutEvent=true&IncludeStatisticsAboutEventPasses=true&IncludeStatisticsAboutFestivals=true" +
            "&IncludeStatisticsAboutReservations=true&IncludeStatisticsAboutPayments=true&IncludeStatisticsAboutUsers=true";

        private static readonly string[] PublicPaths =
        {
            "/api/AdditionalServices", "/api/Equipments", "/api/EventCategories", "/api/EventPassTypes", "/api/FAQ", "/api/SeatTypes",
            "/api/TicketTypes", "/api/PaymentTypes", "/api/MediaPatrons", "/api/Organizers", "/api/Partners", "/api/Sponsors",
            "/api/News", "/api/HallTypes", "/api/Halls", "/api/Events", "/api/Festivals",
        };

        private record Visibility(string Role, string Path, int Status, int[] Ids);

        private static Visibility Ok(string role, string path, params int[] ids) => new(role, path, 200, ids);

        private static Visibility Fails(string role, string path, int status) => new(role, path, status, Array.Empty<int>());

        private static readonly Visibility[] WhoSeesWhat =
        {
            Ok("admin", "/api/Reservations", 1, 2, 3, 4, 5, 6, 7, 8),
            Ok("admin", "/api/EventPasses", 1, 2, 3),
            Ok("admin", "/api/HallRents", 1, 2, 3, 4),
            Ok("admin", "/api/Users/info", 1),
            Ok("user2", "/api/Reservations", 2, 5, 6, 7, 8),
            Ok("user2", "/api/EventPasses", 1),
            Ok("user2", "/api/HallRents", 4),
            Ok("user2", "/api/Users/info", 2),
            Ok("user3", "/api/Reservations", 3, 4),
            Ok("user3", "/api/EventPasses", 2),
            Ok("user3", "/api/HallRents", 2, 3),
            Ok("user3", "/api/Users/info", 3),
            Ok("user4", "/api/Reservations", 1),
            Ok("user4", "/api/EventPasses", 3),
            Ok("user4", "/api/HallRents", 1),
            Ok("user4", "/api/Users/info", 4),
            Ok("user5", "/api/Reservations"),
            Ok("user5", "/api/EventPasses"),
            Ok("user5", "/api/HallRents"),
            Ok("user5", "/api/Users/info", 5),
            Fails("anonymous", "/api/Reservations", 400),
            Ok("anonymous", "/api/Reservations?GetAll=true", 1, 2, 3, 4, 5, 6, 7, 8),
            Fails("anonymous", "/api/HallRents", 400),
            Ok("anonymous", "/api/HallRents?GetAll=true", 1, 2, 3, 4),
            Ok("admin", "/api/Users", 1, 2, 3, 4, 5),
            Ok("admin", "/api/Reservations?GetAll=true", 1, 2, 3, 4, 5, 6, 7, 8),
            Ok("admin", "/api/HallRents?GetAll=true", 1, 2, 3, 4),
            Ok("admin", "/api/EventPasses?GetAll=true", 1, 2, 3),
            Ok("user2", "/api/Reservations/2", 2),
            Fails("user2", "/api/Reservations/3", 403),
            Ok("user2", "/api/EventPasses/1", 1),
            Fails("user2", "/api/EventPasses/2", 403),
            Ok("user2", "/api/HallRents/4", 4),
            Fails("user2", "/api/HallRents/2", 403),
            Ok("user2", "/api/Reservations?GetAll=true", 1, 2, 3, 4, 5, 6, 7, 8),
            Ok("user2", "/api/HallRents?GetAll=true", 1, 2, 3, 4),
            Ok("user3", "/api/Reservations/3", 3),
            Ok("user3", "/api/EventPasses/2", 2),
            Ok("user3", "/api/HallRents/2", 2),
            Ok("user4", "/api/Reservations/1", 1),
            Ok("user4", "/api/EventPasses/3", 3),
            Ok("user4", "/api/HallRents/1", 1),
            Fails("user5", "/api/Users", 403),
            Fails("user5", "/api/Users/5", 403),
            Fails("user5", "/api/Statistics", 403),
        };

        private readonly ApiFactory _factory;

        public SeededResponseTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private async Task<JsonElement> GetAsAsync(string role, string path)
        {
            var client = await _factory.CreateClientForRoleAsync(role);
            return await client.GetJsonAsync(path);
        }

        private static DateTime Day(int offset) => DateTime.Today.AddDays(offset);

        private static int[] Ids(JsonElement parent, string property)
        {
            return parent.GetProperty(property).EnumerateArray().Select(item => item.GetProperty("id").GetId()).ToArray();
        }

        [Fact]
        public async Task Events_list_contains_the_twelve_seeded_events()
        {
            var events = await GetAsAsync("anonymous", "/api/Events");

            events.GetArrayLength().ShouldBe(12);
            events.EnumerateArray().Select(item => item.GetProperty("id").GetId()).ShouldBe(Enumerable.Range(1, 12));
            events.EnumerateArray().Count(item => item.GetProperty("eventStatus").GetString() == "Active").ShouldBe(8);
            events.EnumerateArray().Count(item => item.GetProperty("eventStatus").GetString() == "Expired").ShouldBe(4);
            events[0].GetProperty("title").GetString().ShouldBe("Koncert: Mystic Waves");
        }

        [Fact]
        public async Task Event_1_has_category_hall_and_both_ticket_prices()
        {
            var item = await GetAsAsync("anonymous", "/api/Events/1");

            item.GetProperty("title").GetString().ShouldBe("Koncert: Mystic Waves");
            item.GetProperty("eventStatus").GetString().ShouldBe("Active");
            item.GetProperty("start").GetDateTime().ShouldBe(Day(32));
            item.GetProperty("end").GetDateTime().ShouldBe(Day(32).AddHours(1));
            item.GetProperty("category").GetProperty("name").GetString().ShouldBe("Koncert");
            item.GetProperty("hall").GetProperty("hallNr").GetInt32().ShouldBe(2);
            item.GetProperty("details").GetProperty("id").GetInt32().ShouldBe(1);

            var tickets = item.GetProperty("tickets").EnumerateArray().ToList();
            tickets.Select(ticket => ticket.GetProperty("id").GetInt32()).ShouldBe(new[] { 1, 5 });
            tickets.Select(ticket => ticket.GetProperty("price").GetDecimal()).ShouldBe(new[] { 24.99m, 19.99m });
            tickets.Select(ticket => ticket.GetProperty("isFestival").GetBoolean()).ShouldBe(new[] { false, true });
            tickets.ShouldAllBe(ticket => ticket.GetProperty("ticketType").GetProperty("name").GetString() == "Bilet normalny");
        }

        [Fact]
        public async Task Festival_1_lists_its_events_partners_and_festival_tickets()
        {
            var item = await GetAsAsync("anonymous", "/api/Festivals/1");

            item.GetProperty("title").GetString().ShouldBe("Festiwal Muzyki Hip-Hop");
            item.GetProperty("festivalStatus").GetString().ShouldBe("Active");
            item.GetProperty("start").GetDateTime().ShouldBe(Day(32));
            item.GetProperty("end").GetDateTime().ShouldBe(Day(62).AddHours(1));
            Ids(item, "events").ShouldBe(new[] { 1, 5 });
            Ids(item, "mediaPatrons").ShouldBe(new[] { 1, 2 });
            Ids(item, "organizers").ShouldBe(new[] { 1, 2 });
            Ids(item, "sponsors").ShouldBe(new[] { 1, 2 });
            Ids(item, "tickets").ShouldBe(new[] { 5, 6 });
            item.GetProperty("tickets").EnumerateArray().ShouldAllBe(ticket => ticket.GetProperty("isFestival").GetBoolean());
        }

        [Fact]
        public async Task Hall_1_has_details_type_equipment_and_four_vip_seats()
        {
            var item = await GetAsAsync("anonymous", "/api/Halls/1");

            item.GetProperty("hallNr").GetInt32().ShouldBe(1);
            item.GetProperty("floor").GetInt32().ShouldBe(2);
            item.GetProperty("rentalPricePerHour").GetDecimal().ShouldBe(120.99m);
            item.GetProperty("seatsCount").GetInt32().ShouldBe(4);
            item.GetProperty("hallDetails").GetProperty("totalArea").GetDecimal().ShouldBe(308m);
            item.GetProperty("hallDetails").GetProperty("maxNumberOfSeats").GetInt32().ShouldBe(255);
            item.GetProperty("type").GetProperty("name").GetString().ShouldBe("Sala filmowa");
            Ids(item.GetProperty("type"), "equipments").ShouldBe(new[] { 1, 3, 7 });

            var seats = item.GetProperty("seats").EnumerateArray().ToList();
            seats.Select(seat => seat.GetProperty("seatNr").GetInt32()).ShouldBe(new[] { 1, 2, 3, 4 });
            seats.ShouldAllBe(seat => seat.GetProperty("seatType").GetProperty("name").GetString() == "Miejsce VIP");
        }

        [Fact]
        public async Task Hall_type_1_lists_its_equipment_without_a_photo_endpoint()
        {
            var item = await GetAsAsync("anonymous", "/api/HallTypes/1");

            item.GetProperty("name").GetString().ShouldBe("Sala filmowa");
            item.GetProperty("photoName").GetString().ShouldBe("filmowa.jpg");
            item.GetProperty("photoEndpoint").GetString().ShouldBeEmpty();
            Ids(item, "equipments").ShouldBe(new[] { 1, 3, 7 });
        }

        [Fact]
        public async Task Reservation_2_belongs_to_user_2_and_has_one_regular_seat()
        {
            var item = await GetAsAsync("admin", "/api/Reservations/2");

            item.GetProperty("reservationStatus").GetString().ShouldBe("Active");
            item.GetProperty("paymentAmount").GetDecimal().ShouldBe(34.99m);
            item.GetProperty("reservationDate").GetDateTime().ShouldBe(Day(0));
            item.GetProperty("user").GetProperty("id").GetId().ShouldBe(2);
            item.GetProperty("paymentType").GetProperty("name").GetString().ShouldBe("PayU");
            item.GetProperty("ticket").GetProperty("price").GetDecimal().ShouldBe(34.99m);
            item.GetProperty("ticket").GetProperty("event").GetProperty("title").GetString().ShouldBe("Cień Przeszłości");
            item.GetProperty("ticket").GetProperty("ticketType").GetProperty("name").GetString().ShouldBe("Bilet normalny");

            var seat = item.GetProperty("seats").EnumerateArray().Single();
            seat.GetProperty("id").GetInt32().ShouldBe(38);
            seat.GetProperty("seatType").GetProperty("name").GetString().ShouldBe("Miejsce zwykłe");
        }

        [Fact]
        public async Task Event_pass_1_is_an_active_half_year_pass_of_user_2()
        {
            var item = await GetAsAsync("admin", "/api/EventPasses/1");

            item.GetProperty("eventPassStatus").GetString().ShouldBe("Active");
            item.GetProperty("startDate").GetDateTime().ShouldBe(Day(0));
            item.GetProperty("endDate").GetDateTime().ShouldBe(Day(182));
            item.GetProperty("paymentAmount").GetDecimal().ShouldBe(499.99m);
            item.GetProperty("passType").GetProperty("name").GetString().ShouldBe("Karnet półroczny");
            item.GetProperty("passType").GetProperty("validityPeriodInMonths").GetInt32().ShouldBe(6);
            item.GetProperty("user").GetProperty("id").GetId().ShouldBe(2);
            item.GetProperty("paymentType").GetProperty("name").GetString().ShouldBe("PayU");
        }

        [Fact]
        public async Task Hall_rent_2_is_a_four_hour_rent_of_user_3_with_three_additional_services()
        {
            var item = await GetAsAsync("admin", "/api/HallRents/2");

            item.GetProperty("hallRentStatus").GetString().ShouldBe("Active");
            item.GetProperty("startDate").GetDateTime().ShouldBe(Day(33));
            item.GetProperty("endDate").GetDateTime().ShouldBe(Day(33).AddHours(4));
            item.GetProperty("paymentAmount").GetDecimal().ShouldBe(699.99m);
            item.GetProperty("user").GetProperty("id").GetId().ShouldBe(3);
            item.GetProperty("hall").GetProperty("type").GetProperty("name").GetString().ShouldBe("Sala koncertowa");
            Ids(item, "additionalServices").ShouldBe(new[] { 1, 2, 3 });
        }

        [Fact]
        public async Task User_2_has_profile_counters_address_and_the_user_role()
        {
            var item = await GetAsAsync("admin", "/api/Users/2");

            item.GetProperty("name").GetString().ShouldBe("Mateusz2");
            item.GetProperty("surname").GetString().ShouldBe("Strapczuk2");
            item.GetProperty("emailAddress").GetString().ShouldBe("mateusz.strapczuk2@gmail.com");
            item.GetProperty("allReservationsCount").GetInt32().ShouldBe(5);
            item.GetProperty("allHallRentsCount").GetInt32().ShouldBe(1);
            item.GetProperty("isActiveEventPass").GetBoolean().ShouldBeTrue();
            item.GetProperty("userData").GetProperty("city").GetString().ShouldBe("Poznań");
            item.GetProperty("userRoles").EnumerateArray().Select(role => role.GetString()).ShouldBe(new[] { "User" });
        }

        [Fact]
        public async Task Statistics_of_the_seeded_data_report_expected_totals()
        {
            var report = await GetAsAsync("admin", $"/api/Statistics?{StatisticsWindow}&{AllStatisticsSections}");

            var income = report.GetProperty("totalIncome");
            income.GetProperty("reservationsIncome").GetDecimal().ShouldBe(209.92m);
            income.GetProperty("hallRentsIncome").GetDecimal().ShouldBe(2150.96m);
            income.GetProperty("eventPassesIncome").GetDecimal().ShouldBe(1735.97m);
            income.GetProperty("totalIncome").GetDecimal().ShouldBe(4096.85m);

            var hallRents = report.GetProperty("hallRentStatistics");
            hallRents.GetProperty("allAddedHallRentsCount").GetInt32().ShouldBe(4);
            hallRents.GetProperty("allCanceledHallRentsCount").GetInt32().ShouldBe(0);
            hallRents.GetProperty("totalHallRentsIncome").GetDecimal().ShouldBe(2150.96m);
            hallRents.GetProperty("userReservationsDict").GetProperty("Mateusz3 Strapczuk3").GetInt32().ShouldBe(2);

            var events = report.GetProperty("eventStatistics");
            events.GetProperty("allEventsThatTookPlaceInTimePeriod").GetInt32().ShouldBe(12);
            events.GetProperty("totalEventsIncome").GetDecimal().ShouldBe(109.96m);
            events.GetProperty("mostProfitableEvents").GetProperty("Cień Przeszłości").GetDecimal().ShouldBe(34.99m);

            var passes = report.GetProperty("eventPassStatistics");
            passes.GetProperty("allBougthEventPassesCount").GetInt32().ShouldBe(3);
            passes.GetProperty("totalEventPassesIncome").GetDecimal().ShouldBe(1735.97m);

            var festivals = report.GetProperty("festivalStatistics");
            festivals.GetProperty("allFestivalsThatTookPlaceInTimePeriod").GetInt32().ShouldBe(3);
            festivals.GetProperty("totalFestivalsIncome").GetDecimal().ShouldBe(99.96m);
            festivals.GetProperty("mostProfitableFestivals").GetProperty("Festiwal Filmowy").GetDecimal().ShouldBe(59.98m);

            var reservations = report.GetProperty("reservationStatistics");
            reservations.GetProperty("allNewReservationsCount").GetInt32().ShouldBe(8);
            reservations.GetProperty("allNewEventReservationsCount").GetInt32().ShouldBe(4);
            reservations.GetProperty("allNewFestivalReservationsCount").GetInt32().ShouldBe(4);

            var payments = report.GetProperty("paymentStatistics");
            payments.GetProperty("paymentsCount").GetInt32().ShouldBe(15);
            payments.GetProperty("totalTransactionsCost").GetDecimal().ShouldBe(4096.85m);

            report.GetProperty("userStatistics").GetProperty("totalUsersCount").GetInt32().ShouldBe(5);
        }

        [Fact]
        public async Task Public_lists_and_their_first_item_are_readable_without_login()
        {
            var anonymous = _factory.CreateAnonymousClient();
            var failures = new List<string>();

            foreach (var path in PublicPaths)
            {
                var list = await anonymous.GetAsync(path);
                var item = await anonymous.GetAsync(path + "/1");
                var hasItems = list.IsSuccessStatusCode && JsonDocument.Parse(await list.Content.ReadAsStringAsync()).RootElement.GetArrayLength() > 0;
                if (!hasItems || item.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    failures.Add($"{path}: list {(int)list.StatusCode}, item {(int)item.StatusCode}");
                }
            }

            failures.ShouldBeEmpty();
        }

        [Fact]
        public async Task Every_role_sees_only_the_records_it_is_allowed_to()
        {
            var failures = new List<string>();

            foreach (var expected in WhoSeesWhat)
            {
                var client = await _factory.CreateClientForRoleAsync(expected.Role);
                var response = await client.GetAsync(expected.Path);
                var status = (int)response.StatusCode;
                var ids = status == 200 ? IdsOf(JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement) : Array.Empty<int>();

                if (status != expected.Status || !ids.SequenceEqual(expected.Ids))
                {
                    failures.Add($"{expected.Role} {expected.Path}: expected {expected.Status} [{string.Join(",", expected.Ids)}], got {status} [{string.Join(",", ids)}]");
                }
            }

            failures.ShouldBeEmpty();
        }

        private static int[] IdsOf(JsonElement json)
        {
            var items = json.ValueKind == JsonValueKind.Array ? json.EnumerateArray().ToList() : new List<JsonElement> { json };
            return items.Select(item => item.GetProperty("id").GetId()).ToArray();
        }
    }
}
