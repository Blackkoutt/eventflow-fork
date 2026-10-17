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
            "/api/TicketTypes", "/api/PaymentTypes", "/api/Partners",
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
