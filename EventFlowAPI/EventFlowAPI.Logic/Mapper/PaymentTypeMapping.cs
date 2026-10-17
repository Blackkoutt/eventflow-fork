using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Helpers;

namespace EventFlowAPI.Logic.Mapper;

public static class PaymentTypeMapping
{
    public static PaymentTypeResponseDto ToResponse(this PaymentTypeInfo source) => new()
    {
        Id = source.Id,
        Name = source.Name,
        PhotoName = source.PhotoName
    };
}
