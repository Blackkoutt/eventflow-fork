using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IPartnerService
    {
        Task<Result<PartnerResponseDto>> AddAsync(PartnerRequestDto? requestDto);
        Task<Result<IEnumerable<PartnerResponseDto>>> GetAllAsync(PartnerQuery query);
        Task<Result<PartnerResponseDto>> GetOneAsync(int id);
        Task<Result<PartnerResponseDto>> UpdateAsync(int id, UpdatePartnerRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
        IEnumerable<PartnerResponseDto> MapAsDto(IEnumerable<Partner> records);
        PartnerResponseDto MapAsDto(Partner entity);
    }
}
