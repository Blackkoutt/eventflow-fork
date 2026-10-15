using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Queries
{
    [Collection(ReadOnlyApiCollection.Name)]
    public class SeededResponseSnapshotTests
    {
        private static readonly string StatisticsWindow =
            $"StartDate={DateTime.Today.AddDays(-30):yyyy-MM-dd}&EndDate={DateTime.Today.AddDays(120):yyyy-MM-dd}";

        private static readonly Dictionary<string, (string Role, string Path)> FullResponses = new()
        {
            ["events_list"] = ("anonymous", "/api/Events"),
            ["event_1"] = ("anonymous", "/api/Events/1"),
            ["festival_1"] = ("anonymous", "/api/Festivals/1"),
            ["hall_1"] = ("anonymous", "/api/Halls/1"),
            ["hall_type_1"] = ("anonymous", "/api/HallTypes/1"),
            ["reservation_2"] = ("admin", "/api/Reservations/2"),
            ["event_pass_1"] = ("admin", "/api/EventPasses/1"),
            ["hall_rent_2"] = ("admin", "/api/HallRents/2"),
            ["user_2"] = ("admin", "/api/Users/2"),
            ["statistics_all_sections"] = ("admin", $"/api/Statistics?{StatisticsWindow}&IncludeStatisticsAboutHallRent=true&IncludeStatisticsAboutEvent=true&IncludeStatisticsAboutEventPasses=true&IncludeStatisticsAboutFestivals=true&IncludeStatisticsAboutReservations=true&IncludeStatisticsAboutPayments=true&IncludeStatisticsAboutUsers=true"),
        };

        private static readonly string[] PublicPaths =
        {
            "/api/AdditionalServices", "/api/Equipments", "/api/EventCategories", "/api/EventPassTypes", "/api/FAQ", "/api/SeatTypes",
            "/api/TicketTypes", "/api/PaymentTypes", "/api/MediaPatrons", "/api/Organizers", "/api/Partners", "/api/Sponsors",
            "/api/News", "/api/HallTypes", "/api/Halls", "/api/Events", "/api/Festivals",
        };

        private static readonly string[] OwnLists =
        {
            "/api/Reservations", "/api/EventPasses", "/api/HallRents", "/api/Users/info",
        };

        private static readonly (string Role, string Path)[] ScopedRequests =
        {
            ("anonymous", "/api/Reservations"),
            ("anonymous", "/api/Reservations?GetAll=true"),
            ("anonymous", "/api/HallRents"),
            ("anonymous", "/api/HallRents?GetAll=true"),
            ("admin", "/api/Users"),
            ("admin", "/api/Reservations?GetAll=true"),
            ("admin", "/api/HallRents?GetAll=true"),
            ("admin", "/api/EventPasses?GetAll=true"),
            ("user2", "/api/Reservations/2"),
            ("user2", "/api/Reservations/3"),
            ("user2", "/api/EventPasses/1"),
            ("user2", "/api/EventPasses/2"),
            ("user2", "/api/HallRents/4"),
            ("user2", "/api/HallRents/2"),
            ("user2", "/api/Reservations?GetAll=true"),
            ("user2", "/api/HallRents?GetAll=true"),
            ("user3", "/api/Reservations/3"),
            ("user3", "/api/EventPasses/2"),
            ("user3", "/api/HallRents/2"),
            ("user4", "/api/Reservations/1"),
            ("user4", "/api/EventPasses/3"),
            ("user4", "/api/HallRents/1"),
            ("user5", "/api/Users"),
            ("user5", "/api/Users/5"),
            ("user5", "/api/Statistics"),
        };

        public static IEnumerable<object[]> Names
        {
            get
            {
                foreach (var name in FullResponses.Keys)
                {
                    yield return new object[] { name };
                }
            }
        }

        private readonly ApiFactory _factory;
        private readonly SnapshotNormalizer _normalizer = new();

        public SeededResponseSnapshotTests(ApiFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [MemberData(nameof(Names))]
        public async Task Full_response_matches_snapshot(string name)
        {
            var request = FullResponses[name];
            var client = await _factory.CreateClientForRoleAsync(request.Role);

            var response = await client.GetAsync(request.Path);

            var snapshot = new JsonObject
            {
                ["status"] = (int)response.StatusCode,
                ["body"] = _normalizer.Normalize(await response.Content.ReadAsStringAsync()),
            };
            await Verifier.VerifyJson(snapshot.ToJsonString())
                .UseDirectory("SeededResponses")
                .UseTextForParameters(name);
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
        public async Task Who_sees_what_matches_snapshot()
        {
            var lines = new StringBuilder();
            foreach (var role in new[] { "admin", "user2", "user3", "user4", "user5" })
            {
                var client = await _factory.CreateClientForRoleAsync(role);
                foreach (var path in OwnLists)
                {
                    lines.AppendLine(await DescribeAsync(client, role, path));
                }
            }

            foreach (var request in ScopedRequests)
            {
                var client = await _factory.CreateClientForRoleAsync(request.Role);
                lines.AppendLine(await DescribeAsync(client, request.Role, request.Path));
            }

            await Verifier.Verify(lines.ToString()).UseDirectory("SeededResponses");
        }

        private async Task<string> DescribeAsync(ApiClient client, string role, string path)
        {
            var response = await client.GetAsync(path);
            var status = (int)response.StatusCode;
            if (status != 200)
            {
                return $"{role} {path} => {status}";
            }

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            var ids = new List<string>();
            var items = json.ValueKind == JsonValueKind.Array ? json.EnumerateArray().ToList() : new List<JsonElement> { json };
            foreach (var item in items)
            {
                ids.Add(item.GetProperty("id").GetRawText().Trim('"'));
            }
            return $"{role} {path} => {status} [{string.Join(",", ids)}]";
        }
    }
}
