using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface ITicketTypeService
    {
        Task<Result<TicketTypeResponseDto>> AddAsync(TicketTypeRequestDto? requestDto);
        Task<Result<IEnumerable<TicketTypeResponseDto>>> GetAllAsync(TicketTypeQuery query);
        Task<Result<TicketTypeResponseDto>> GetOneAsync(int id);
        Task<Result<TicketTypeResponseDto>> UpdateAsync(int id, UpdateTicketTypeRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
    }
}
