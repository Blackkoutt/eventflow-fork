using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IFAQService
    {
        Task<Result<FAQResponseDto>> AddAsync(FAQRequestDto? requestDto);
        Task<Result<IEnumerable<FAQResponseDto>>> GetAllAsync(FAQQuery query);
        Task<Result<FAQResponseDto>> GetOneAsync(int id);
        Task<Result<FAQResponseDto>> UpdateAsync(int id, UpdateFAQRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
    }
}
