using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.DTO.Abstract;
using System.Text.Json.Serialization;

namespace EventFlowAPI.Logic.DTO.ResponseDto
{
    public class PartnerResponseDto : BaseResponseDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PartnerKind Kind { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PhotoName { get; set; } = string.Empty;
        public string PhotoEndpoint { get; set; } = string.Empty;
    }
}
