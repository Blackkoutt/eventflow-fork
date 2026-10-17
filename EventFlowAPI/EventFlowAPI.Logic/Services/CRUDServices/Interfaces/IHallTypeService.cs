using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IHallTypeService
    {
        Task<Result<HallTypeResponseDto>> AddAsync(HallTypeRequestDto? requestDto);
        Task<Result<IEnumerable<HallTypeResponseDto>>> GetAllAsync(HallTypeQuery query);
        Task<Result<HallTypeResponseDto>> GetOneAsync(int id);
        Task<Result<HallTypeResponseDto>> UpdateAsync(int id, UpdateHallTypeRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
        IEnumerable<HallTypeResponseDto> MapAsDto(IEnumerable<HallType> records);
        HallTypeResponseDto MapAsDto(HallType entity);
    }
}
