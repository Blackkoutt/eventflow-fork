using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IAdditionalServicesService
    {
        Task<Result<AdditionalServicesResponseDto>> AddAsync(AdditionalServicesRequestDto? requestDto);
        Task<Result<IEnumerable<AdditionalServicesResponseDto>>> GetAllAsync(AdditionalServicesQuery query);
        Task<Result<AdditionalServicesResponseDto>> GetOneAsync(int id);
        Task<Result<AdditionalServicesResponseDto>> UpdateAsync(int id, UpdateAdditionalServicesRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
    }
}
