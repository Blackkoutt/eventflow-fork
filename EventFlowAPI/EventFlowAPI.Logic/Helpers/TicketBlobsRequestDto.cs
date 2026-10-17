using EventFlowAPI.Logic.Enums;

namespace EventFlowAPI.Logic.Helpers
{
    public class TicketBlobsRequestDto
    {
        public Guid ReservationGuid { get; set; }
        public string Folder { get; set; } = string.Empty;
        public BlobContainer Container { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public List<byte[]> DataList { get; set; } = [];
    }
}
