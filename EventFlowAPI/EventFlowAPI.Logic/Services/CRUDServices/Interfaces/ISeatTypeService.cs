using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface ISeatTypeService
    {
        Task<Result<SeatTypeResponseDto>> AddAsync(SeatTypeRequestDto? requestDto);
        Task<Result<IEnumerable<SeatTypeResponseDto>>> GetAllAsync(SeatTypeQuery query);
        Task<Result<SeatTypeResponseDto>> GetOneAsync(int id);
        Task<Result<SeatTypeResponseDto>> UpdateAsync(int id, UpdateSeatTypeRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
    }
}
