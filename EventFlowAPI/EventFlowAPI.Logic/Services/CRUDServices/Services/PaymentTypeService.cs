using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class PaymentTypeService(IFileService fileService) : IPaymentTypeService
    {
        private readonly IFileService _fileService = fileService;

        public async Task<Result<IEnumerable<PaymentTypeResponseDto>>> GetAllAsync(PaymentTypeQuery query)
        {
            var records = PaymentTypes.All.AsQueryable()
                                          .ByName(query)
                                          .SortBy(query.SortBy, query.SortDirection)
                                          .GetPage(query.PageNumber, query.PageSize)
                                          .ToList();
            var response = records.Select(MapAsDto);
            return await Task.FromResult(Result<IEnumerable<PaymentTypeResponseDto>>.Success(response));
        }

        public async Task<Result<PaymentTypeResponseDto>> GetOneAsync(int id)
        {
            if (id < 0)
                return await Task.FromResult(Result<PaymentTypeResponseDto>.Failure(Error.RouteParamOutOfRange));

            var record = PaymentTypes.All.FirstOrDefault(paymentType => paymentType.Id == id);
            if (record == null)
                return await Task.FromResult(Result<PaymentTypeResponseDto>.Failure(Error.NotFound));

            return await Task.FromResult(Result<PaymentTypeResponseDto>.Success(MapAsDto(record)));
        }

        public async Task<Result<BlobResponseDto>> GetImageAsync(int id)
        {
            if (id < 0)
                return Result<BlobResponseDto>.Failure(Error.RouteParamOutOfRange);

            var record = PaymentTypes.All.FirstOrDefault(paymentType => paymentType.Id == id);
            if (record == null)
                return Result<BlobResponseDto>.Failure(Error.NotFound);

            return await _fileService.GetEntityPhoto(record);
        }

        private PaymentTypeResponseDto MapAsDto(PaymentType entity)
        {
            var responseDto = entity.AsDto<PaymentTypeResponseDto>();
            responseDto.PhotoEndpoint = $"/PaymentTypes/{responseDto.Id}/image";
            return responseDto;
        }
    }
}
