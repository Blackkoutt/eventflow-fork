using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IEventPassService
    {
        Task<Result<EventPassResponseDto>> AddAsync(EventPassRequestDto? requestDto);
        Task<Result<IEnumerable<EventPassResponseDto>>> GetAllAsync(EventPassQuery query);
        Task<Result<EventPassResponseDto>> GetOneAsync(int id);
        Task<Result<EventPassResponseDto>> UpdateAsync(int id, UpdateEventPassRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
        Task<Result<EventPassResponseDto>> BuyEventPass(EventPassRequestDto? requestDto);
        Task<Result<EventPassResponseDto>> RenewEventPass(int id, UpdateEventPassRequestDto? requestDto);
    }
}
