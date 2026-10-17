using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class PartnerTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;

        public PartnerTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private static string UniqueName()
        {
            return "Partner " + Guid.NewGuid().ToString("N")[..8];
        }

        [Fact]
        public async Task Creating_without_a_kind_returns_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.PostFormAsync("/api/Partners", new FormData().Add("Name", UniqueName()));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("MediaPatron")]
        [InlineData("Organizer")]
        [InlineData("Sponsor")]
        [InlineData("Partner")]
        public async Task A_created_partner_is_listed_only_under_its_kind(string kind)
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var name = UniqueName();

            var created = await admin.PostFormAsync("/api/Partners", new FormData().Add("Name", name).Add("Kind", kind));

            created.StatusCode.ShouldBe(HttpStatusCode.Created, await admin.ReadErrorsAsync(created));
            var id = (await admin.ReadJsonAsync(created)).GetProperty("id").GetInt32();
            foreach (var listedKind in new[] { "MediaPatron", "Organizer", "Sponsor", "Partner" })
            {
                var listed = await admin.GetJsonAsync($"/api/Partners?Kind={listedKind}&Name={name}");
                var ids = listed.EnumerateArray().Select(item => item.GetProperty("id").GetInt32()).ToList();
                ids.Contains(id).ShouldBe(listedKind == kind);
            }
        }

        [Fact]
        public async Task The_same_name_is_allowed_for_different_kinds_but_not_twice_within_one_kind()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var name = UniqueName();

            var sponsor = await admin.PostFormAsync("/api/Partners", new FormData().Add("Name", name).Add("Kind", "Sponsor"));
            var organizer = await admin.PostFormAsync("/api/Partners", new FormData().Add("Name", name).Add("Kind", "Organizer"));
            var duplicate = await admin.PostFormAsync("/api/Partners", new FormData().Add("Name", name).Add("Kind", "Sponsor"));

            sponsor.StatusCode.ShouldBe(HttpStatusCode.Created);
            organizer.StatusCode.ShouldBe(HttpStatusCode.Created);
            duplicate.StatusCode.ShouldNotBe(HttpStatusCode.Created);
            var sponsors = await admin.GetJsonAsync($"/api/Partners?Kind=Sponsor&Name={name}");
            sponsors.GetArrayLength().ShouldBe(1);
        }
    }
}
