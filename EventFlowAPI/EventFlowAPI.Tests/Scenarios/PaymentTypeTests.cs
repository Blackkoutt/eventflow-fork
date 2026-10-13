using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class PaymentTypeTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;

        public PaymentTypeTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private string UniqueName()
        {
            return "Platnosc " + Guid.NewGuid().ToString("N")[..8];
        }

        private async Task<int> CreateAsync(ApiClient admin, FormData form)
        {
            var response = await admin.PostFormAsync("/api/PaymentTypes", form);
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await admin.ReadErrorsAsync(response));
            return (await admin.ReadJsonAsync(response)).GetProperty("id").GetInt32();
        }

        [Fact]
        public async Task Created_payment_type_is_listed_and_has_a_default_image()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var id = await CreateAsync(admin, new FormData().Add("Name", UniqueName()));

            new ApiLookups(admin).ReadIds(await admin.GetJsonAsync("/api/PaymentTypes")).ShouldContain(id);
            var image = await admin.GetAsync($"/api/PaymentTypes/{id}/image");
            image.StatusCode.ShouldBe(HttpStatusCode.OK);
            image.Content.Headers.ContentType!.MediaType.ShouldBe("image/jpeg");
        }

        [Fact]
        public async Task Uploaded_photo_is_served_back()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var id = await CreateAsync(admin, new FormData().Add("Name", UniqueName()).AddFile("PaymentTypePhoto", "photo.png", TestImages.Png, "image/png"));

            var image = await admin.GetAsync($"/api/PaymentTypes/{id}/image");
            image.Content.Headers.ContentType!.MediaType.ShouldBe("image/png");
        }

        [Fact]
        public async Task Duplicate_name_currently_returns_500_with_a_409_body()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var name = UniqueName();
            await CreateAsync(admin, new FormData().Add("Name", name));

            var response = await admin.PostFormAsync("/api/PaymentTypes", new FormData().Add("Name", name));

            response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
            (await admin.ReadJsonAsync(response)).GetProperty("code").GetInt32().ShouldBe(409);
        }

        [Fact]
        public async Task Wrong_photo_extension_and_missing_name_return_400()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var badPhoto = await admin.PostFormAsync("/api/PaymentTypes", new FormData().Add("Name", UniqueName()).AddFile("PaymentTypePhoto", "photo.txt", TestImages.Text, "text/plain"));
            var noName = await admin.PostFormAsync("/api/PaymentTypes", new FormData());

            badPhoto.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            noName.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Deleting_removes_the_payment_type()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var id = await CreateAsync(admin, new FormData().Add("Name", UniqueName()));

            var response = await admin.DeleteAsync($"/api/PaymentTypes/{id}");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            (await admin.GetAsync($"/api/PaymentTypes/{id}")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
            (await admin.GetAsync($"/api/PaymentTypes/{id}/image")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Fact]
        [Trait(TestCategories.Name, TestCategories.DatabaseSpecific)]
        public async Task Deleting_a_referenced_payment_type_currently_succeeds()
        {
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.DeleteAsync("/api/PaymentTypes/2");

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
    }
}
