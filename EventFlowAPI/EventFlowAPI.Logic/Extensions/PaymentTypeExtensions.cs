using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Mapper.Extensions;

namespace EventFlowAPI.Logic.Extensions
{
    public static class PaymentTypeExtensions
    {
        public static PaymentTypeResponseDto AsDto(this PaymentType paymentType)
        {
            var info = PaymentTypeCatalogue.All.First(item => item.Id == (int)paymentType);
            return info.AsDto<PaymentTypeResponseDto>();
        }
    }
}
