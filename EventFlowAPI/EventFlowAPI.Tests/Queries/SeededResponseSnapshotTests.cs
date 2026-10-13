using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Queries
{
    [Collection(ReadOnlyApiCollection.Name)]
    public class SeededResponseSnapshotTests
    {
        private static readonly string StatisticsWindow =
            $"StartDate={DateTime.Today.AddDays(-30):yyyy-MM-dd}&EndDate={DateTime.Today.AddDays(120):yyyy-MM-dd}";

        private static readonly string[] PublicPaths =
        {
            "/api/AdditionalServices", "/api/AdditionalServices/1",
            "/api/Equipments", "/api/Equipments/1",
            "/api/EventCategories", "/api/EventCategories/1",
            "/api/EventPassTypes", "/api/EventPassTypes/1",
            "/api/FAQ", "/api/FAQ/1",
            "/api/SeatTypes", "/api/SeatTypes/1",
            "/api/TicketTypes", "/api/TicketTypes/1",
            "/api/PaymentTypes", "/api/PaymentTypes/1",
            "/api/MediaPatrons", "/api/MediaPatrons/1",
            "/api/Organizers", "/api/Organizers/1",
            "/api/Partners", "/api/Partners/1",
            "/api/Sponsors", "/api/Sponsors/1",
            "/api/News", "/api/News/1",
            "/api/HallTypes", "/api/HallTypes/1",
            "/api/Halls", "/api/Halls/1", "/api/Halls/5",
            "/api/Events", "/api/Events/1", "/api/Events/9",
            "/api/Festivals", "/api/Festivals/1", "/api/Festivals/3",
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
            ("admin", "/api/Reservations/2"),
            ("admin", "/api/EventPasses/1"),
            ("admin", "/api/HallRents/2"),
            ("admin", "/api/Users"),
            ("admin", "/api/Users/2"),
            ("admin", "/api/Reservations?GetAll=true"),
            ("admin", "/api/HallRents?GetAll=true"),
            ("admin", "/api/EventPasses?GetAll=true"),
            ("user2", "/api/Reservations/2"),
            ("user2", "/api/EventPasses/1"),
            ("user2", "/api/HallRents/4"),
            ("user2", "/api/Reservations?GetAll=true"),
            ("user2", "/api/HallRents?GetAll=true"),
            ("user3", "/api/Reservations/3"),
            ("user3", "/api/EventPasses/2"),
            ("user3", "/api/HallRents/2"),
            ("user4", "/api/Reservations/1"),
            ("user4", "/api/EventPasses/3"),
            ("user4", "/api/HallRents/1"),
            ("admin", $"/api/Statistics?{StatisticsWindow}"),
            ("admin", $"/api/Statistics?{StatisticsWindow}&IncludeStatisticsAboutHallRent=true&IncludeStatisticsAboutEvent=true&IncludeStatisticsAboutEventPasses=true&IncludeStatisticsAboutFestivals=true&IncludeStatisticsAboutReservations=true&IncludeStatisticsAboutPayments=true&IncludeStatisticsAboutUsers=true"),
        };

        public static IEnumerable<object[]> Requests
        {
            get
            {
                foreach (var path in PublicPaths)
                {
                    yield return new object[] { "anonymous", path };
                }

                foreach (var role in new[] { "admin", "user2", "user3", "user4", "user5" })
                {
                    foreach (var path in OwnLists)
                    {
                        yield return new object[] { role, path };
                    }
                }

                foreach (var request in ScopedRequests)
                {
                    yield return new object[] { request.Role, request.Path };
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
        [MemberData(nameof(Requests))]
        public async Task Seeded_response_matches_snapshot(string role, string path)
        {
            var client = await _factory.CreateClientForRoleAsync(role);

            var response = await client.GetAsync(path);

            var snapshot = new JsonObject
            {
                ["status"] = (int)response.StatusCode,
                ["body"] = _normalizer.Normalize(await response.Content.ReadAsStringAsync()),
            };
            await Verifier.VerifyJson(snapshot.ToJsonString())
                .UseDirectory("SeededResponses")
                .UseTextForParameters(BuildName(role, path));
        }

        private string BuildName(string role, string path)
        {
            var readable = Regex.Replace($"{role}{path}", "[^A-Za-z0-9]+", "_").Trim('_');
            if (readable.Length <= 70)
            {
                return readable;
            }

            var hash = Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(path)))[..8];
            return $"{readable[..60]}_{hash}";
        }
    }
}
