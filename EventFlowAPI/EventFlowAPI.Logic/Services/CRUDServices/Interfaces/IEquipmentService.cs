using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IEquipmentService
    {
        Task<Result<EquipmentResponseDto>> AddAsync(EquipmentRequestDto? requestDto);
        Task<Result<IEnumerable<EquipmentResponseDto>>> GetAllAsync(EquipmentQuery query);
        Task<Result<EquipmentResponseDto>> GetOneAsync(int id);
        Task<Result<EquipmentResponseDto>> UpdateAsync(int id, UpdateEquipmentRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
    }
}
