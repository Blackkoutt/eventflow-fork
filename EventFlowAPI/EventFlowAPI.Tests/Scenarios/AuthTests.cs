using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using EventFlowAPI.Tests.Infrastructure;

namespace EventFlowAPI.Tests.Scenarios
{
    public class AuthTests : IClassFixture<ApiFactory>
    {
        private readonly ApiFactory _factory;

        public AuthTests(ApiFactory factory)
        {
            _factory = factory;
        }

        private object NewUser(string email, string password = "Haslo123!", string confirm = "Haslo123!", string dateOfBirth = "2000-01-01")
        {
            return new { name = "Jan", surname = "Kowalski", email, password, confirmPassword = confirm, dateOfBirth };
        }

        [Fact]
        public async Task Login_with_wrong_password_returns_401()
        {
            var client = _factory.CreateAnonymousClient();

            var response = await client.PostAsync("/api/Auth/login", new { email = TestUsers.Admin.Email, password = "zle" });

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            (await client.ReadErrorsAsync(response)).ShouldBe("Nieprawidłowy login lub hasło.");
        }

        [Fact]
        public async Task Login_with_unknown_email_returns_401()
        {
            var client = _factory.CreateAnonymousClient();

            var response = await client.PostAsync("/api/Auth/login", new { email = "nobody@example.com", password = "zle" });

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_returns_token_for_valid_credentials()
        {
            var client = _factory.CreateAnonymousClient();

            var response = await client.PostAsync("/api/Auth/login", new { email = TestUsers.Admin.Email, password = TestUsers.Admin.Password });

            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            var body = await client.ReadJsonAsync(response);
            body.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Validate_requires_a_valid_token()
        {
            var anonymous = _factory.CreateAnonymousClient();
            var admin = await _factory.CreateClientAsync(TestUsers.Admin);

            (await anonymous.GetAsync("/api/Auth/validate")).StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            (await admin.GetAsync("/api/Auth/validate")).StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Garbage_token_is_rejected()
        {
            var httpClient = _factory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "not.a.token");

            var response = await httpClient.GetAsync("/api/Auth/validate");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Registered_user_can_log_in_and_is_listed_for_admin()
        {
            var anonymous = _factory.CreateAnonymousClient();

            var register = await anonymous.PostAsync("/api/Auth/register", NewUser("rejestracja@example.com"));
            register.StatusCode.ShouldBe(HttpStatusCode.OK);

            var login = await anonymous.PostAsync("/api/Auth/login", new { email = "rejestracja@example.com", password = "Haslo123!" });
            login.StatusCode.ShouldBe(HttpStatusCode.OK);

            var admin = await _factory.CreateClientAsync(TestUsers.Admin);
            var users = await admin.GetJsonAsync("/api/Users");
            users.EnumerateArray().Select(user => user.GetProperty("emailAddress").GetString())
                .ShouldContain("rejestracja@example.com");
        }

        [Fact]
        public async Task Registered_user_has_no_admin_rights()
        {
            var anonymous = _factory.CreateAnonymousClient();
            await anonymous.PostAsync("/api/Auth/register", NewUser("zwykly@example.com"));
            var httpClient = _factory.CreateClient();
            var login = await httpClient.PostAsJsonAsync("/api/Auth/login", new { email = "zwykly@example.com", password = "Haslo123!" });
            var token = (await anonymous.ReadJsonAsync(login)).GetProperty("token").GetString();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("/api/Users");

            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Registering_a_taken_email_returns_400()
        {
            var client = _factory.CreateAnonymousClient();

            var response = await client.PostAsync("/api/Auth/register", NewUser(TestUsers.Admin.Email));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            (await client.ReadErrorsAsync(response)).ShouldBe("Adres e-mail jest już zajęty. Użyj innego.");
        }

        [Fact]
        public async Task Registering_with_mismatched_passwords_returns_400()
        {
            var client = _factory.CreateAnonymousClient();

            var response = await client.PostAsync("/api/Auth/register", NewUser("a1@example.com", confirm: "Inne123!"));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Registering_with_too_short_password_returns_400()
        {
            var client = _factory.CreateAnonymousClient();

            var response = await client.PostAsync("/api/Auth/register", NewUser("a2@example.com", "abc", "abc"));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Registering_a_user_younger_than_13_returns_400()
        {
            var client = _factory.CreateAnonymousClient();
            var birthDate = DateTime.Today.AddYears(-10).ToString("yyyy-MM-dd");

            var response = await client.PostAsync("/api/Auth/register", NewUser("a3@example.com", dateOfBirth: birthDate));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Registering_with_invalid_email_returns_400()
        {
            var client = _factory.CreateAnonymousClient();

            var response = await client.PostAsync("/api/Auth/register", NewUser("nie-email"));

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
