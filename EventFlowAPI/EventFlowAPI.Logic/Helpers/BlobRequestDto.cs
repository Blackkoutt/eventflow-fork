using EventFlowAPI.Logic.Enums;

namespace EventFlowAPI.Logic.Helpers
{
    public class BlobRequestDto
    {
        public BlobContainer Container { get; set; }
        public string Folder { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = [];
        public CancellationToken CancellationToken { get; set; } = default;
    }
}
