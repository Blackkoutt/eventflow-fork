using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class SnapshotNormalizer
    {
        private static readonly Regex DateTimePattern = new(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?$");

        public JsonNode? Normalize(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                return Visit(JsonNode.Parse(json));
            }
            catch (System.Text.Json.JsonException)
            {
                return JsonValue.Create(json);
            }
        }

        private JsonNode? Visit(JsonNode? node)
        {
            if (node is JsonObject source)
            {
                var result = new JsonObject();
                foreach (var property in source)
                {
                    result[property.Key] = Visit(property.Value);
                }
                return result;
            }

            if (node is JsonArray list)
            {
                var result = new JsonArray();
                foreach (var item in list)
                {
                    result.Add(Visit(item));
                }
                return result;
            }

            if (node is JsonValue value && value.TryGetValue<string>(out var text) && DateTimePattern.IsMatch(text))
            {
                var date = DateTime.Parse(text, CultureInfo.InvariantCulture);
                var offset = (date.Date - DateTime.Today).Days;
                return JsonValue.Create($"today{offset:+0;-0;+0}d {date:HH:mm:ss}");
            }

            return node?.DeepClone();
        }
    }
}
