using System.Text.Json;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    [Collection(ReadOnlyApiCollection.Name)]
    public class StatisticsTests
    {
        private static readonly Dictionary<string, string> SectionsByFlag = new()
        {
            ["IncludeStatisticsAboutHallRent"] = "hallRentStatistics",
            ["IncludeStatisticsAboutEvent"] = "eventStatistics",
            ["IncludeStatisticsAboutEventPasses"] = "eventPassStatistics",
            ["IncludeStatisticsAboutFestivals"] = "festivalStatistics",
            ["IncludeStatisticsAboutReservations"] = "reservationStatistics",
            ["IncludeStatisticsAboutPayments"] = "paymentStatistics",
            ["IncludeStatisticsAboutUsers"] = "userStatistics",
        };

        public static IEnumerable<object[]> Flags
        {
            get
            {
                foreach (var flag in SectionsByFlag.Keys)
                {
                    yield return new object[] { flag };
                }
            }
        }

        private readonly ApiFactory _factory;

        public StatisticsTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private string Window()
        {
            return $"StartDate={DateTime.Today.AddDays(-30):yyyy-MM-dd}&EndDate={DateTime.Today.AddDays(120):yyyy-MM-dd}";
        }

        [Theory]
        [MemberData(nameof(Flags))]
        public async Task Each_flag_returns_only_its_own_section(string flag)
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var json = await admin.GetJsonAsync($"/api/Statistics?{Window()}&{flag}=true");

            foreach (var section in SectionsByFlag)
            {
                var expected = section.Key == flag ? JsonValueKind.Object : JsonValueKind.Null;
                json.GetProperty(section.Value).ValueKind.ShouldBe(expected, section.Value);
            }
        }
    }
}
