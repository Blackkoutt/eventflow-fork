using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Queries
{
    [Collection(ReadOnlyApiCollection.Name)]
    public class DerivedValueTests
    {
        private readonly ApiFactory _factory;

        public DerivedValueTests(ApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Event_duration_is_the_difference_between_its_dates()
        {
            var anonymous = _factory.CreateAnonymousClient();

            var duration = (await anonymous.GetJsonAsync("/api/Events/1")).GetProperty("duration").GetString();

            duration.ShouldBe("01:00:00");
        }

        [Fact]
        public async Task Festival_duration_is_the_difference_between_its_dates()
        {
            var anonymous = _factory.CreateAnonymousClient();

            var festival = await anonymous.GetJsonAsync("/api/Festivals/1");

            var start = festival.GetProperty("start").GetDateTime();
            var end = festival.GetProperty("end").GetDateTime();
            TimeSpan.Parse(festival.GetProperty("duration").GetString()!).ShouldBe(end - start);
        }

        [Fact]
        public async Task Hall_area_is_length_times_width()
        {
            var anonymous = _factory.CreateAnonymousClient();

            var details = (await anonymous.GetJsonAsync("/api/Halls/5")).GetProperty("hallDetails");

            details.GetProperty("totalArea").GetDecimal()
                .ShouldBe(details.GetProperty("totalLength").GetDecimal() * details.GetProperty("totalWidth").GetDecimal());
        }
    }
}
