using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IPaymentTypeService
    {
        Task<Result<IEnumerable<PaymentTypeResponseDto>>> GetAllAsync(PaymentTypeQuery query);
        Task<Result<PaymentTypeResponseDto>> GetOneAsync(int id);
        Task<Result<BlobResponseDto>> GetImageAsync(int id);
    }
}
