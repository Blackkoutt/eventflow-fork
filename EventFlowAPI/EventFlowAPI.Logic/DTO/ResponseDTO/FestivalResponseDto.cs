using EventFlowAPI.Logic.DTO.Abstract;
using EventFlowAPI.Logic.Enums;
using System.Text.Json.Serialization;

namespace EventFlowAPI.Logic.DTO.ResponseDto
{
    public class FestivalResponseDto : BaseResponseDto
    {
        [JsonPropertyName("title")]
        public string Name { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        [JsonPropertyName("start")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("end")]
        public DateTime EndDate { get; set; }
        public Status FestivalStatus { get; set; }
        public TimeSpan Duration { get; set; }
        public string PhotoName { get; set; } = string.Empty;
        public string PhotoEndpoint { get; set; } = string.Empty;
        public string? LongDescription { get; set; }
        public ICollection<EventResponseDto> Events { get; set; } = [];
        public ICollection<PartnerResponseDto> MediaPatrons { get; set; } = [];
        public ICollection<PartnerResponseDto> Organizers { get; set; } = [];
        public ICollection<PartnerResponseDto> Sponsors { get; set; } = [];
        public ICollection<TicketResponseDto> Tickets { get; set; } = [];
    }
}
