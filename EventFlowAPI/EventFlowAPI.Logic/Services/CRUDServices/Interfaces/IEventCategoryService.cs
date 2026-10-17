using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IEventCategoryService
    {
        Task<Result<EventCategoryResponseDto>> AddAsync(EventCategoryRequestDto? requestDto);
        Task<Result<IEnumerable<EventCategoryResponseDto>>> GetAllAsync(EventCategoryQuery query);
        Task<Result<EventCategoryResponseDto>> GetOneAsync(int id);
        Task<Result<EventCategoryResponseDto>> UpdateAsync(int id, UpdateEventCategoryRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
    }
}
