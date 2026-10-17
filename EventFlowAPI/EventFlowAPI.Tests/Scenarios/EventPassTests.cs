using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class EventPassTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public EventPassTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        [Fact]
        public async Task Buying_a_pass_creates_an_active_pass_visible_in_the_owners_list()
        {
            var user = await _world.RegisterUserAsync();

            var passId = await _world.BuyPassAsync(user.Client);

            var detail = await user.Client.GetJsonAsync($"/api/EventPasses/{passId}");
            detail.GetProperty("eventPassStatus").GetString().ShouldBe("Active");
            detail.GetProperty("renewalDate").ValueKind.ShouldBe(System.Text.Json.JsonValueKind.Null);
            detail.GetProperty("paymentAmount").GetDecimal().ShouldBe(89.99m);
            detail.GetProperty("endDate").GetDateTime().ShouldBe(detail.GetProperty("startDate").GetDateTime().AddMonths(1), TimeSpan.FromSeconds(1));
            var list = await user.Client.GetJsonAsync("/api/EventPasses");
            new ApiLookups(user.Client).ReadIds(list).ShouldBe(new[] { passId });
        }

        [Fact]
        public async Task Own_info_currently_reports_no_active_pass_while_the_admin_user_detail_reports_one()
        {
            var user = await _world.RegisterUserAsync();
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            await _world.BuyPassAsync(user.Client);

            var info = await user.Client.GetJsonAsync("/api/Users/info");
            var detail = await admin.GetJsonAsync($"/api/Users/{user.Id}");

            info.GetProperty("isActiveEventPass").GetBoolean().ShouldBeFalse();
            detail.GetProperty("isActiveEventPass").GetBoolean().ShouldBeTrue();
        }

        [Fact]
        public async Task Buying_a_second_pass_while_one_is_active_returns_400()
        {
            var user = await _world.RegisterUserAsync();
            await _world.BuyPassAsync(user.Client);

            var response = await user.Client.PostAsync("/api/EventPasses", new { passTypeId = 2 });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await user.Client.ReadErrorsAsync(response)).ShouldBe("Użytkownik posiada już aktywny karnet.");
        }

        [Fact]
        public async Task Buying_a_pass_of_an_unknown_type_returns_400()
        {
            var user = await _world.RegisterUserAsync();

            var response = await user.Client.PostAsync("/api/EventPasses", new { passTypeId = 999 });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Admin_can_currently_buy_a_pass()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.PostAsync("/api/EventPasses", new { passTypeId = 1 });

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Renewing_own_pass_extends_it_and_charges_the_new_price_with_the_old_type_discount()
        {
            var user = await _world.RegisterUserAsync();
            var passId = await _world.BuyPassAsync(user.Client);
            var before = await user.Client.GetJsonAsync($"/api/EventPasses/{passId}");

            var response = await user.Client.PutAsync($"/api/EventPasses/{passId}", new { passTypeId = 2 });

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var after = await user.Client.GetJsonAsync($"/api/EventPasses/{passId}");
            after.GetProperty("renewalDate").ValueKind.ShouldNotBe(System.Text.Json.JsonValueKind.Null);
            after.GetProperty("endDate").GetDateTime().ShouldBe(before.GetProperty("endDate").GetDateTime().AddMonths(3));
            after.GetProperty("paymentAmount").GetDecimal().ShouldBe(224.19m);
        }

        [Fact]
        public async Task Renewing_someone_elses_pass_returns_403_but_admin_may()
        {
            var user = await _world.RegisterUserAsync();
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var passId = await _world.BuyPassAsync(user.Client);
            var stranger = await _world.RegisterUserAsync();

            var forbidden = await stranger.Client.PutAsync($"/api/EventPasses/{passId}", new { passTypeId = 2 });
            var allowed = await admin.PutAsync($"/api/EventPasses/{passId}", new { passTypeId = 2 });

            forbidden.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            allowed.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Reading_someone_elses_pass_returns_403_and_an_unknown_one_404()
        {
            var user = await _world.RegisterUserAsync();

            (await user.Client.GetAsync("/api/EventPasses/2")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            (await user.Client.GetAsync("/api/EventPasses/999")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Only_admin_can_cancel_a_pass_and_cancelling_allows_buying_again()
        {
            var user = await _world.RegisterUserAsync();
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var passId = await _world.BuyPassAsync(user.Client);

            var byUser = await user.Client.DeleteAsync($"/api/EventPasses/{passId}");
            var byAdmin = await admin.DeleteAsync($"/api/EventPasses/{passId}");
            var detail = await user.Client.GetJsonAsync($"/api/EventPasses/{passId}");
            var renew = await user.Client.PutAsync($"/api/EventPasses/{passId}", new { passTypeId = 1 });
            var buyAgain = await user.Client.PostAsync("/api/EventPasses", new { passTypeId = 1 });

            byUser.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            byAdmin.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            detail.GetProperty("eventPassStatus").GetString().ShouldBe("Canceled");
            renew.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await user.Client.ReadErrorsAsync(renew)).ShouldBe("Karnet został anulowany.");
            buyAgain.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Cancelling_an_already_canceled_pass_returns_404()
        {
            var user = await _world.RegisterUserAsync();
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var passId = await _world.BuyPassAsync(user.Client);
            await admin.DeleteAsync($"/api/EventPasses/{passId}");

            var response = await admin.DeleteAsync($"/api/EventPasses/{passId}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}
