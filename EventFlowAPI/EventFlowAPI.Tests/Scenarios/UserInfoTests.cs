using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class UserInfoTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;
        private readonly ScenarioWorld _world;

        public UserInfoTests(ApiFactory factory)
        {
            _factory = factory;
            _world = new ScenarioWorld(factory);
        }

        private FormData Address(string city)
        {
            return new FormData().Add("Street", "Dluga").Add("HouseNumber", 5).Add("FlatNumber", 7).Add("City", city).Add("ZipCode", "60-001").Add("PhoneNumber", "123456789");
        }

        [Fact]
        public async Task Updating_info_stores_the_address()
        {
            var user = await _factory.CreateClientAsync(TestUsers.User5);

            var response = await user.PutFormAsync("/api/Users/info", Address("Poznan"));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var address = (await user.GetJsonAsync("/api/Users/info"));
            address.GetProperty("city").GetString().ShouldBe("Poznan");
            address.GetProperty("street").GetString().ShouldBe("Dluga");
            address.GetProperty("zipCode").GetString().ShouldBe("60-001");
        }

        [Fact]
        public async Task Uploading_a_user_photo_replaces_the_image()
        {
            var user = await _world.RegisterUserAsync();

            var response = await user.Client.PutFormAsync("/api/Users/info", Address("Krakow").AddFile("UserPhoto", "me.png", TestImages.Png, "image/png"));

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var image = await user.Client.GetAsync("/api/Users/info/image");
            image.StatusCode.ShouldBe(HttpStatusCode.OK);
            image.Content.Headers.ContentType!.MediaType.ShouldBe("image/png");
        }

        [Fact]
        public async Task Anonymous_info_update_currently_returns_400_because_the_endpoint_has_no_authorization()
        {
            var response = await _factory.CreateAnonymousClient().PutFormAsync("/api/Users/info", Address("Poznan"));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await _factory.CreateAnonymousClient().ReadErrorsAsync(response)).ShouldBe("Użytkownik nie ma ID w claims");
        }

        [Fact]
        public async Task Seeded_user_image_is_public()
        {
            var image = await _factory.CreateAnonymousClient().GetAsync("/api/Users/2/image");

            image.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task User_detail_is_admin_only_even_for_the_own_account()
        {
            var user = await _factory.CreateClientAsync(TestUsers.User5);
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            (await user.GetAsync("/api/Users/5")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            (await user.GetAsync("/api/Users/2")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            (await user.GetAsync("/api/Users")).StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            (await admin.GetAsync("/api/Users/2")).StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
