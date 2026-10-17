using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces.BaseInterfaces;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IPartnerService : IGenericService<
            Partner,
            PartnerRequestDto,
            UpdatePartnerRequestDto,
            PartnerResponseDto,
            PartnerQuery
        >
    {
        IEnumerable<PartnerResponseDto> MapAsDto(IEnumerable<Partner> records);
        PartnerResponseDto MapAsDto(Partner entity);
    }
}
