using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IHallService
    {
        Task<Result<HallResponseDto>> AddAsync(HallRequestDto? requestDto);
        Task<Result<IEnumerable<HallResponseDto>>> GetAllAsync(HallQuery query);
        Task<Result<HallResponseDto>> GetOneAsync(int id);
        Task<Result<HallResponseDto>> UpdateAsync(int id, UpdateHallRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
        Task<Result<HallResponseDto>> UpdateHallForEvent(int eventId, EventHallRequestDto? requestDto);
        Task<Result<HallResponseDto>> UpdateHallForRent(int rentId, HallRent_HallRequestDto? requestDto);
    }
}
