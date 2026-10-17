using System.Text.Json;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class ApiLookups
    {
        public const int NormalTicketTypeId = 1;
        public const int ReducedTicketTypeId = 2;

        private readonly ApiClient _client;

        public ApiLookups(ApiClient client)
        {
            _client = client;
        }

        public async Task<int> GetNormalTicketIdAsync(int eventId)
        {
            return await GetTicketIdAsync($"/api/Events/{eventId}", NormalTicketTypeId);
        }

        public async Task<int> GetFestivalTicketIdAsync(int festivalId)
        {
            return await GetTicketIdAsync($"/api/Festivals/{festivalId}", NormalTicketTypeId);
        }

        public async Task<int> GetTicketIdAsync(string ownerUrl, int ticketTypeId)
        {
            var owner = await _client.GetJsonAsync(ownerUrl);
            var isFestival = ownerUrl.Contains("/Festivals/");
            foreach (var ticket in owner.GetProperty("tickets").EnumerateArray())
            {
                var typeMatches = ticket.GetProperty("ticketType").GetProperty("id").GetInt32() == ticketTypeId;
                if (typeMatches && ticket.GetProperty("isFestival").GetBoolean() == isFestival)
                {
                    return ticket.GetProperty("id").GetInt32();
                }
            }

            throw new InvalidOperationException($"{ownerUrl} has no ticket of type {ticketTypeId}");
        }

        public async Task<List<int>> GetSeatIdsAsync(int eventId)
        {
            var eventJson = await _client.GetJsonAsync($"/api/Events/{eventId}");
            var hallId = eventJson.GetProperty("hall").GetProperty("id").GetInt32();
            return await GetHallSeatIdsAsync(hallId);
        }

        public async Task<List<int>> GetHallSeatIdsAsync(int hallId)
        {
            var hall = await _client.GetJsonAsync($"/api/Halls/{hallId}");
            return ReadIds(hall.GetProperty("seats"));
        }

        public List<int> ReadIds(JsonElement list)
        {
            var result = new List<int>();
            foreach (var item in list.EnumerateArray())
            {
                result.Add(item.GetProperty("id").GetInt32());
            }

            return result;
        }

        public List<(int TicketTypeId, decimal Price)> ReadTickets(JsonElement owner)
        {
            var result = new List<(int TicketTypeId, decimal Price)>();
            foreach (var ticket in owner.GetProperty("tickets").EnumerateArray())
            {
                result.Add((ticket.GetProperty("ticketType").GetProperty("id").GetInt32(), ticket.GetProperty("price").GetDecimal()));
            }

            return result.OrderBy(item => item.TicketTypeId).ThenBy(item => item.Price).ToList();
        }

        public async Task<string> GetStatusAsync(string url, string property)
        {
            return (await _client.GetJsonAsync(url)).GetProperty(property).GetString()!;
        }
    }
}
