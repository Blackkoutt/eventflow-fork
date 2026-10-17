using System.Net;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public record PhotoCase(string Route, string CreatePhotoField, string UpdatePhotoField, string NameField, bool Updatable = true, bool HasSeededImage = true);

    public class PhotoEntityTests : IClassFixture<ApiFactory>
    {
        private static readonly Dictionary<string, PhotoCase> Cases = new()
        {
            ["MediaPatrons"] = new("MediaPatrons", "MediaPatronPhoto", "MediaPatronPhoto", "Name"),
            ["Organizers"] = new("Organizers", "OrganizerPhoto", "OrganizerPhoto", "Name"),
            ["Sponsors"] = new("Sponsors", "SponsorPhoto", "SponosorPhoto", "Name"),
            ["Partners"] = new("Partners", "PartnerPhoto", "PartnerPhoto", "Name"),
            ["News"] = new("News", "NewsPhoto", "NewsPhoto", "Title"),
            ["HallTypes"] = new("HallTypes", "HallTypePhoto", "HallTypePhoto", "Name"),
            ["PaymentTypes"] = new("PaymentTypes", "PaymentTypePhoto", "PaymentTypePhoto", "Name", Updatable: false, HasSeededImage: false),
        };

        public static IEnumerable<object[]> Routes => Keys(_ => true);

        public static IEnumerable<object[]> UpdatableRoutes => Keys(photoCase => photoCase.Updatable);

        public static IEnumerable<object[]> SeededImageRoutes => Keys(photoCase => photoCase.HasSeededImage);

        private static IEnumerable<object[]> Keys(Func<PhotoCase, bool> filter)
        {
            return Cases.Where(item => filter(item.Value)).Select(item => new object[] { item.Key });
        }

        private readonly ApiFactory _factory;

        public PhotoEntityTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private FormData Form(PhotoCase photoCase, string name)
        {
            var form = new FormData().Add(photoCase.NameField, name);
            if (photoCase.Route == "News")
            {
                form.Add("ShortDescription", "krotki opis").Add("LongDescription", "dlugi opis wiadomosci");
            }
            if (photoCase.Route == "HallTypes")
            {
                form.Add("Description", "opis").Add("EquipmentIds", 1).Add("EquipmentIds", 2);
            }
            return form;
        }

        private string UniqueName()
        {
            return "Wpis " + Guid.NewGuid().ToString("N")[..8];
        }

        private async Task<int> CreateAsync(ApiClient admin, PhotoCase photoCase, string name, FormData? form = null)
        {
            var response = await admin.PostFormAsync($"/api/{photoCase.Route}", form ?? Form(photoCase, name));
            response.StatusCode.ShouldBe(HttpStatusCode.Created, await admin.ReadErrorsAsync(response));
            return (await admin.ReadJsonAsync(response)).GetProperty("id").GetInt32();
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Item_without_photo_gets_the_default_image(string key)
        {
            var photoCase = Cases[key];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var id = await CreateAsync(admin, photoCase, UniqueName());

            var image = await admin.GetAsync($"/api/{photoCase.Route}/{id}/image");
            image.StatusCode.ShouldBe(HttpStatusCode.OK);
            image.Content.Headers.ContentType!.MediaType.ShouldBe("image/jpeg");
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Uploaded_photo_is_served_back(string key)
        {
            var photoCase = Cases[key];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var name = UniqueName();

            var id = await CreateAsync(admin, photoCase, name, Form(photoCase, name).AddFile(photoCase.CreatePhotoField, "photo.png", TestImages.Png, "image/png"));

            var image = await _factory.CreateAnonymousClient().GetAsync($"/api/{photoCase.Route}/{id}/image");
            image.StatusCode.ShouldBe(HttpStatusCode.OK);
            image.Content.Headers.ContentType!.MediaType.ShouldBe("image/png");
            (await image.Content.ReadAsByteArrayAsync()).Length.ShouldBe(TestImages.Png.Length);
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Photo_with_a_wrong_extension_returns_400(string key)
        {
            var photoCase = Cases[key];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.PostFormAsync($"/api/{photoCase.Route}",
                Form(photoCase, UniqueName()).AddFile(photoCase.CreatePhotoField, "photo.txt", TestImages.Text, "text/plain"));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await admin.ReadErrorsAsync(response)).ShouldBe("Niepoprawne rozszerzenie pliku. Spróbuj przesłać plik w formacie jpeg lub png.");
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Creating_without_data_returns_400(string key)
        {
            var photoCase = Cases[key];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.PostFormAsync($"/api/{photoCase.Route}", new FormData());

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Theory]
        [MemberData(nameof(UpdatableRoutes))]
        public async Task Updating_changes_the_name_and_replaces_the_photo(string key)
        {
            var photoCase = Cases[key];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var id = await CreateAsync(admin, photoCase, UniqueName());
            var newName = UniqueName();

            var renamed = await admin.PutFormAsync($"/api/{photoCase.Route}/{id}", Form(photoCase, newName));
            var withPhoto = await admin.PutFormAsync($"/api/{photoCase.Route}/{id}",
                Form(photoCase, newName).AddFile(photoCase.UpdatePhotoField, "photo.png", TestImages.Png, "image/png"));

            renamed.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            withPhoto.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            var detail = await admin.GetJsonAsync($"/api/{photoCase.Route}/{id}");
            detail.GetProperty(photoCase.NameField.ToLowerInvariant()).GetString().ShouldBe(newName);
        }

        [Theory]
        [MemberData(nameof(UpdatableRoutes))]
        public async Task Updating_an_unknown_item_returns_404(string key)
        {
            var photoCase = Cases[key];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            var response = await admin.PutFormAsync($"/api/{photoCase.Route}/9999", Form(photoCase, UniqueName()));

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(Routes))]
        public async Task Deleting_removes_the_item_and_its_image(string key)
        {
            var photoCase = Cases[key];
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var id = await CreateAsync(admin, photoCase, UniqueName());

            var delete = await admin.DeleteAsync($"/api/{photoCase.Route}/{id}");

            delete.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            (await admin.GetAsync($"/api/{photoCase.Route}/{id}")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
            (await admin.GetAsync($"/api/{photoCase.Route}/{id}/image")).StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }

        [Theory]
        [MemberData(nameof(SeededImageRoutes))]
        public async Task Seeded_image_is_served(string key)
        {
            var photoCase = Cases[key];
            var anonymous = _factory.CreateAnonymousClient();

            var image = await anonymous.GetAsync($"/api/{photoCase.Route}/2/image");

            image.StatusCode.ShouldBe(HttpStatusCode.OK);
            (await image.Content.ReadAsByteArrayAsync()).Length.ShouldBeGreaterThan(1000);
        }
    }
}
