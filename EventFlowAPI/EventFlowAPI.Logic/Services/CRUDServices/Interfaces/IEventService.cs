using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IEventService
    {
        Task<Result<EventResponseDto>> AddAsync(EventRequestDto? requestDto);
        Task<Result<IEnumerable<EventResponseDto>>> GetAllAsync(EventQuery query);
        Task<Result<EventResponseDto>> GetOneAsync(int id);
        Task<Result<EventResponseDto>> UpdateAsync(int id, UpdateEventRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
        Task<Result<(IEnumerable<Reservation> ReservationsForEvent, Event NewEvent, Event OldEvent)>> UpdateEvent(int id, UpdateEventRequestDto? requestDto); 
    }
}
