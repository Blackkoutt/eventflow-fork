using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class PaymentTypeMapping
{
    public static PaymentType ToEntity(this PaymentTypeRequestDto source) => source is null ? null! : source.ApplyTo(new PaymentType());

    public static PaymentType ApplyTo(this PaymentTypeRequestDto source, PaymentType target)
    {
        target.Name = source.Name;
        return target;
    }

    public static PaymentType ToEntity(this UpdatePaymentTypeRequestDto source) => source is null ? null! : source.ApplyTo(new PaymentType());

    public static PaymentType ApplyTo(this UpdatePaymentTypeRequestDto source, PaymentType target)
    {
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static PaymentTypeResponseDto? ToResponse(this PaymentType? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static PaymentTypeResponseDto? ToResponse(this PaymentType? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<PaymentTypeResponseDto>(source, out var existing)) return existing;
        var target = new PaymentTypeResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.PhotoName = source.PhotoName;
        target.Id = source.Id;
        return target;
    }
}
