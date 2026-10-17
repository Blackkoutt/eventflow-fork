using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface INewsService
    {
        Task<Result<NewsResponseDto>> AddAsync(NewsRequestDto? requestDto);
        Task<Result<IEnumerable<NewsResponseDto>>> GetAllAsync(NewsQuery query);
        Task<Result<NewsResponseDto>> GetOneAsync(int id);
        Task<Result<NewsResponseDto>> UpdateAsync(int id, UpdateNewsRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
    }
}
