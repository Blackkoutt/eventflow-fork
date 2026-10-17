using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IFestivalService
    {
        Task<Result<FestivalResponseDto>> AddAsync(FestivalRequestDto? requestDto);
        Task<Result<IEnumerable<FestivalResponseDto>>> GetAllAsync(FestivalQuery query);
        Task<Result<FestivalResponseDto>> GetOneAsync(int id);
        Task<Result<FestivalResponseDto>> UpdateAsync(int id, UpdateFestivalRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
        Task<ICollection<Festival>> CancelFestivalIfEssential(IEnumerable<Event> eventsToDelete);
    }
}
