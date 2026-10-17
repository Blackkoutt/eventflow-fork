using System.Net.Http.Json;
using System.Text.Json;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> SendAsync(string method, string url)
        {
            return await _httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(method), url));
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object? body = null)
        {
            return await _httpClient.PostAsJsonAsync(url, body ?? new { });
        }

        public async Task<HttpResponseMessage> PutAsync(string url, object? body = null)
        {
            return await _httpClient.PutAsJsonAsync(url, body ?? new { });
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _httpClient.DeleteAsync(url);
        }

        public async Task<HttpResponseMessage> PostFormAsync(string url, FormData form)
        {
            return await _httpClient.PostAsync(url, form.ToContent());
        }

        public async Task<HttpResponseMessage> PutFormAsync(string url, FormData form)
        {
            return await _httpClient.PutAsync(url, form.ToContent());
        }

        public async Task<JsonElement> GetJsonAsync(string url)
        {
            var response = await GetAsync(url);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK, $"GET {url}");
            return await ReadJsonAsync(response);
        }

        public async Task<JsonElement> ReadJsonAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(content).RootElement.Clone();
        }

        public async Task<string> ReadErrorsAsync(HttpResponseMessage response)
        {
            if (response.Content.Headers.ContentLength == 0)
            {
                return "";
            }

            var mediaType = response.Content.Headers.ContentType?.MediaType;
            if (mediaType != null && !mediaType.Contains("json"))
            {
                return mediaType;
            }

            var json = await ReadJsonAsync(response);
            if (json.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in json.EnumerateObject())
                {
                    if (string.Equals(property.Name, "details", StringComparison.OrdinalIgnoreCase)
                        && property.Value.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var details in property.Value.EnumerateObject())
                        {
                            if (string.Equals(details.Name, "errors", StringComparison.OrdinalIgnoreCase))
                            {
                                return string.Join(" | ", details.Value.EnumerateArray().Select(error => error.GetString()));
                            }
                        }
                    }
                }
            }

            return json.ToString();
        }
    }
}
