using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public record LookupCase(string Route, Dictionary<string, object> Create, Dictionary<string, object> Update, object Invalid, string UpdatedField, HttpStatusCode SeededDelete);

    public class LookupCrudTests : IClassFixture<ApiFactory>
    {
        private static readonly Dictionary<string, LookupCase> Cases = new()
        {
            ["AdditionalServices"] = new("AdditionalServices",
                new Dictionary<string, object> { ["name"] = "Nowa usluga", ["price"] = 10.5, ["description"] = "opis" },
                new Dictionary<string, object> { ["name"] = "Nowa usluga", ["price"] = 12.25, ["description"] = "nowy opis" },
                new { name = "", price = -1 }, "description", HttpStatusCode.NoContent),
            ["Equipments"] = new("Equipments",
                new Dictionary<string, object> { ["name"] = "Nowy sprzet", ["description"] = "opis" },
                new Dictionary<string, object> { ["name"] = "Nowy sprzet", ["description"] = "nowy" },
                new { name = "" }, "description", HttpStatusCode.NoContent),
            ["EventCategories"] = new("EventCategories",
                new Dictionary<string, object> { ["name"] = "Nowa kategoria", ["icon"] = "fa-solid fa-star", ["color"] = "#112233" },
                new Dictionary<string, object> { ["name"] = "Nowa kategoria", ["icon"] = "fa-solid fa-star", ["color"] = "#445566" },
                new { name = "", icon = "", color = "" }, "color", HttpStatusCode.InternalServerError),
            ["EventPassTypes"] = new("EventPassTypes",
                new Dictionary<string, object> { ["name"] = "Karnet testowy", ["validityPeriodInMonths"] = 2, ["renewalDiscountPercentage"] = 7.5, ["price"] = 150.0 },
                new Dictionary<string, object> { ["name"] = "Karnet testowy", ["validityPeriodInMonths"] = 2, ["renewalDiscountPercentage"] = 8.0, ["price"] = 160.0 },
                new { name = "", validityPeriodInMonths = 0, price = -5 }, "price", HttpStatusCode.NoContent),
            ["FAQ"] = new("FAQ",
                new Dictionary<string, object> { ["question"] = "Czy dziala?", ["answer"] = "Tak" },
                new Dictionary<string, object> { ["question"] = "Czy dziala?", ["answer"] = "Na pewno" },
                new { question = "", answer = "" }, "answer", HttpStatusCode.NoContent),
            ["SeatTypes"] = new("SeatTypes",
                new Dictionary<string, object> { ["name"] = "Miejsce testowe", ["description"] = "opis", ["seatColor"] = "#AABBCC", ["addtionalPaymentPercentage"] = 5.5 },
                new Dictionary<string, object> { ["name"] = "Miejsce testowe", ["description"] = "nowy opis", ["seatColor"] = "#DDEEFF", ["addtionalPaymentPercentage"] = 6.5 },
                new { name = "", seatColor = "zly", addtionalPaymentPercentage = -1 }, "description", HttpStatusCode.InternalServerError),
            ["TicketTypes"] = new("TicketTypes",
                new Dictionary<string, object> { ["name"] = "Bilet testowy" },
                new Dictionary<string, object> { ["name"] = "Bilet testowy zmieniony" },
                new { name = "" }, "name", HttpStatusCode.InternalServerError),
        };

        public static IEnumerable<object[]> Routes
        {
            get
            {
                foreach (var key in Cases.Keys)
                {
                    yield return new object[] { key };
                }
            }
        }

        private readonly ApiFactory _factory;

        public LookupCrudTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private Dictionary<string, object> WithSuffix(Dictionary<string, object> payload, string suffix)
        {
            var isUpdate = ReferenceEquals(payload, Cases.Values.FirstOrDefault(item => ReferenceEquals(item.Update, payload))?.Update);
            var copy = new Dictionary<string, object>(payload);
            var uniqueKey = copy.ContainsKey("name") ? "name" : "question";
            copy[uniqueKey] = copy[uniqueKey] + suffix;
            if (copy.ContainsKey("seatColor"))
            {
                copy["seatColor"] = "#" + (isUpdate ? new string(suffix.Reverse().ToArray()) : suffix);
            }
            return copy;
        }

        private async Task<int> CreateAsync(ApiClient admin, LookupCase lookup, string suffix)
        {
            var response = await admin.PostAsync($"/api/{lookup.Route}", WithSuffix(lookup.Create, suffix));
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await admin.ReadErrorsAsync(response));
            return (await admin.ReadJsonAsync(response)).GetProperty("id").GetInt32();
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Created_item_can_be_read_back_and_listed(string key)
        {
            var lookup = Cases[key];
            var suffix = Guid.NewGuid().ToString("N")[..6];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var anonymous = _factory.CreateAnonymousClient();

            var id = await CreateAsync(admin, lookup, suffix);

            (await anonymous.GetAsync($"/api/{lookup.Route}/{id}")).StatusCode.ShouldBe(HttpStatusCode.OK);
            new ApiLookups(anonymous).ReadIds(await anonymous.GetJsonAsync($"/api/{lookup.Route}")).ShouldContain(id);
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Creating_a_duplicate_name_currently_returns_500_with_a_409_body(string key)
        {
            var lookup = Cases[key];
            var suffix = Guid.NewGuid().ToString("N")[..6];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            await CreateAsync(admin, lookup, suffix);

            var response = await admin.PostAsync($"/api/{lookup.Route}", WithSuffix(lookup.Create, suffix));

            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            (await admin.ReadJsonAsync(response)).GetProperty("code").GetInt32().ShouldBe(409);
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Creating_an_invalid_item_returns_400(string key)
        {
            var lookup = Cases[key];
            var suffix = Guid.NewGuid().ToString("N")[..6];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.PostAsync($"/api/{lookup.Route}", lookup.Invalid);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Updating_an_item_changes_it(string key)
        {
            var lookup = Cases[key];
            var suffix = Guid.NewGuid().ToString("N")[..6];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var id = await CreateAsync(admin, lookup, suffix);
            var before = await admin.GetJsonAsync($"/api/{lookup.Route}/{id}");

            var response = await admin.PutAsync($"/api/{lookup.Route}/{id}", WithSuffix(lookup.Update, suffix));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var after = await admin.GetJsonAsync($"/api/{lookup.Route}/{id}");
            after.GetProperty(lookup.UpdatedField).ToString().ShouldNotBe(before.GetProperty(lookup.UpdatedField).ToString());
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Updating_an_unknown_item_returns_404(string key)
        {
            var lookup = Cases[key];
            var suffix = Guid.NewGuid().ToString("N")[..6];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.PutAsync($"/api/{lookup.Route}/9999", WithSuffix(lookup.Update, suffix));

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Deleting_an_item_removes_it(string key)
        {
            var lookup = Cases[key];
            var suffix = Guid.NewGuid().ToString("N")[..6];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var id = await CreateAsync(admin, lookup, suffix);

            var first = await admin.DeleteAsync($"/api/{lookup.Route}/{id}");
            var read = await admin.GetAsync($"/api/{lookup.Route}/{id}");
            var second = await admin.DeleteAsync($"/api/{lookup.Route}/{id}");

            first.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            read.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            second.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [Trait(TestCategories.Name, TestCategories.DatabaseSpecific)]
        [MemberData(nameof(Routes))]
        public async Task Deleting_a_seeded_item_gives_the_current_result(string key)
        {
            var lookup = Cases[key];
            var suffix = Guid.NewGuid().ToString("N")[..6];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.DeleteAsync($"/api/{lookup.Route}/1");

            response.StatusCode.ShouldBe(lookup.SeededDelete);
        }
    }
}
