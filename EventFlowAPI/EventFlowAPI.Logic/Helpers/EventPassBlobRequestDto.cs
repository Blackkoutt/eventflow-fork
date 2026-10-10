namespace EventFlowAPI.Logic.Helpers
{
    public class EventPassBlobRequestDto
    {
        public Guid EventPassGuid { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = [];
    }
}
