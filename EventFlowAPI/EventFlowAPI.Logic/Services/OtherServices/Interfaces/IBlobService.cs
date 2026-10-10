using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.ResultObject;

namespace EventFlowAPI.Logic.Services.OtherServices.Interfaces
{
    public interface IBlobService
    {
        Task<Result<string>> CreateEventPassBlob(EventPassBlobRequestDto request);
        Task<Result<List<IFileEntity>>> CreateTicketBlobs(TicketBlobsRequestDto request);
        Task<Result<object>> UploadAsync(BlobRequestDto blob);
        Task<Result<BlobResponseDto>> DownloadAsync(BlobRequestDto blob);
        Task DeleteAsync(BlobRequestDto blob);
        Task<Result<object>> CreateBlob(BlobRequestDto blob);


    }
}
