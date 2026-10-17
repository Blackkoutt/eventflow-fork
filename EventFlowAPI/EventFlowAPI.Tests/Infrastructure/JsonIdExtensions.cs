using System.Text.Json;

namespace EventFlowAPI.Tests.Infrastructure
{
    public static class JsonIdExtensions
    {
        // Entity ids are numbers, user ids are serialized as strings.
        public static int GetId(this JsonElement element)
        {
            return element.ValueKind == JsonValueKind.String ? int.Parse(element.GetString()!) : element.GetInt32();
        }
    }
}
