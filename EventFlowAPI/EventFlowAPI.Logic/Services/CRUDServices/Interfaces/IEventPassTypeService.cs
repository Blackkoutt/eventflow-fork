using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IEventPassTypeService
    {
        Task<Result<EventPassTypeResponseDto>> AddAsync(EventPassTypeRequestDto? requestDto);
        Task<Result<IEnumerable<EventPassTypeResponseDto>>> GetAllAsync(EventPassTypeQuery query);
        Task<Result<EventPassTypeResponseDto>> GetOneAsync(int id);
        Task<Result<EventPassTypeResponseDto>> UpdateAsync(int id, UpdateEventPassTypeRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
    }
}
