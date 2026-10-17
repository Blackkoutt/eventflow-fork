using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.Abstract;
using EventFlowAPI.Logic.Enums;
using System.Text.Json.Serialization;

namespace EventFlowAPI.Logic.DTO.ResponseDto
{
    public class EventResponseDto : BaseResponseDto
    {
        [JsonPropertyName("title")]
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;

        [JsonPropertyName("start")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end")]
        public DateTime EndDate { get; set; }
        public TimeSpan Duration { get; set; }
        public Status EventStatus { get; set; }
        public string PhotoName { get; set; } = string.Empty;
        public string PhotoEndpoint { get; set; } = string.Empty;
        public EventCategoryResponseDto? Category { get; set; }
        public string? LongDescription { get; set; }
        public HallResponseDto? Hall { get; set; }
        public ICollection<TicketResponseDto> Tickets { get; set; } = [];
    }
}
