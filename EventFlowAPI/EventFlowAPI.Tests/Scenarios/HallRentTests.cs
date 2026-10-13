using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class HallRentTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public HallRentTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private async Task<int> RentOkAsync(ApiClient client, DateTime start, int hallId = 2, int[]? services = null)
        {
            var response = await _world.RentAsync(client, start, hallId: hallId, services: services);
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await client.ReadErrorsAsync(response));
            return (await client.ReadJsonAsync(response)).GetProperty("id").GetInt32();
        }

        [Fact]
        public async Task Renting_a_hall_creates_a_rent_visible_to_the_owner()
        {
            var user = await _world.RegisterUserAsync();
            var start = _world.NextFarStart();

            var rentId = await RentOkAsync(user.Client, start, services: new[] { 2, 3 });

            var detail = await user.Client.GetJsonAsync($"/api/HallRents/{rentId}");
            detail.GetProperty("user").GetProperty("id").GetString().ShouldBe(user.Id);
            detail.GetProperty("startDate").GetDateTime().ShouldBe(start);
            detail.GetProperty("deleteDate").ValueKind.ShouldBe(System.Text.Json.JsonValueKind.Null);
            detail.GetProperty("additionalServices").GetArrayLength().ShouldBe(2);
            var list = await user.Client.GetJsonAsync("/api/HallRents");
            new ApiLookups(user.Client).ReadIds(list).ShouldBe(new[] { rentId });
        }

        [Fact]
        public async Task Rent_price_is_hourly_hall_price_plus_selected_services()
        {
            var user = await _world.RegisterUserAsync();
            var start = _world.NextFarStart();
            var withoutServices = await RentOkAsync(user.Client, start);
            var withServices = await RentOkAsync(user.Client, start.AddHours(5), services: new[] { 2, 3 });

            var plain = await user.Client.GetJsonAsync($"/api/HallRents/{withoutServices}");
            var extended = await user.Client.GetJsonAsync($"/api/HallRents/{withServices}");

            plain.GetProperty("paymentAmount").GetDecimal().ShouldBe(179.98m);
            extended.GetProperty("paymentAmount").GetDecimal().ShouldBe(769.98m);
        }

        [Fact]
        public async Task Overlapping_rents_of_the_same_hall_are_currently_accepted()
        {
            var first = await _world.RegisterUserAsync();
            var second = await _world.RegisterUserAsync();
            var start = _world.NextFarStart();
            await RentOkAsync(first.Client, start);

            var overlapping = await _world.RentAsync(second.Client, start.AddHours(1));

            overlapping.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Renting_a_hall_during_a_seeded_event_is_currently_accepted()
        {
            var user = await _world.RegisterUserAsync();
            var eventStart = DateTime.Today.AddDays(34);

            var response = await _world.RentAsync(user.Client, eventStart, 1, hallId: 1);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task Rent_ending_before_it_starts_returns_400()
        {
            var user = await _world.RegisterUserAsync();
            var start = _world.NextFarStart();

            var response = await _world.RentAsync(user.Client, start, -2);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Rent_in_the_past_returns_400()
        {
            var user = await _world.RegisterUserAsync();

            var response = await _world.RentAsync(user.Client, DateTime.Today.AddDays(-5).AddHours(10));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Rent_of_an_unknown_hall_returns_400()
        {
            var user = await _world.RegisterUserAsync();
            var start = _world.NextFarStart();

            var response = await _world.RentAsync(user.Client, start, hallId: 999);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await user.Client.ReadErrorsAsync(response)).ShouldBe("Sala o podanym ID nie istnieje w bazie danych.");
        }

        [Fact]
        public async Task Rent_with_an_unknown_service_returns_404()
        {
            var user = await _world.RegisterUserAsync();
            var start = _world.NextFarStart();

            var response = await _world.RentAsync(user.Client, start, services: new[] { 999 });

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Admin_can_rent_a_hall()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await _world.RentAsync(admin, _world.NextFarStart());

            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task A_user_can_currently_make_many_rents_in_one_month()
        {
            var user = await _world.RegisterUserAsync();
            var slot = _factory.NextSlot();

            foreach (var index in Enumerable.Range(0, 6))
            {
                var response = await _world.RentAsync(user.Client, _world.SlotStart(slot).AddHours(index * 3), hallId: 3);
                response.StatusCode.ShouldBe(HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task Reading_another_users_rent_returns_403()
        {
            var owner = await _world.RegisterUserAsync();
            var stranger = await _world.RegisterUserAsync();
            var rentId = await RentOkAsync(owner.Client, _world.NextFarStart());

            var response = await stranger.Client.GetAsync($"/api/HallRents/{rentId}");

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task User_list_contains_only_own_rents_and_admin_list_contains_all()
        {
            var user = await _world.RegisterUserAsync();
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var rentId = await RentOkAsync(user.Client, _world.NextFarStart());

            var userList = new ApiLookups(user.Client).ReadIds(await user.Client.GetJsonAsync("/api/HallRents"));
            var adminList = new ApiLookups(admin).ReadIds(await admin.GetJsonAsync("/api/HallRents"));

            userList.ShouldBe(new[] { rentId });
            adminList.ShouldContain(rentId);
            adminList.ShouldContain(1);
        }

        [Fact]
        public async Task Deleting_own_rent_soft_deletes_it()
        {
            var owner = await _world.RegisterUserAsync();
            var stranger = await _world.RegisterUserAsync();
            var rentId = await RentOkAsync(owner.Client, _world.NextFarStart());

            var byStranger = await stranger.Client.DeleteAsync($"/api/HallRents/{rentId}");
            var byOwner = await owner.Client.DeleteAsync($"/api/HallRents/{rentId}");
            var again = await owner.Client.DeleteAsync($"/api/HallRents/{rentId}");
            var detail = await owner.Client.GetJsonAsync($"/api/HallRents/{rentId}");

            byStranger.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            byOwner.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            again.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            detail.GetProperty("deleteDate").ValueKind.ShouldNotBe(System.Text.Json.JsonValueKind.Null);
        }

        [Fact]
        public async Task Admin_can_delete_a_seeded_rent()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.DeleteAsync("/api/HallRents/1");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
    }
}
