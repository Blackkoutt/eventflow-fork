using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class EventPassTypeMapping
{
    public static EventPassType ToEntity(this EventPassTypeRequestDto source) => source is null ? null! : source.ApplyTo(new EventPassType());

    public static EventPassType ApplyTo(this EventPassTypeRequestDto source, EventPassType target)
    {
        target.Name = source.Name;
        target.ValidityPeriodInMonths = source.ValidityPeriodInMonths;
        target.Price = source.Price;
        target.RenewalDiscountPercentage = source.RenewalDiscountPercentage;
        return target;
    }

    public static EventPassType ToEntity(this UpdateEventPassTypeRequestDto source) => source is null ? null! : source.ApplyTo(new EventPassType());

    public static EventPassType ApplyTo(this UpdateEventPassTypeRequestDto source, EventPassType target)
    {
        target.Price = source.Price;
        target.RenewalDiscountPercentage = source.RenewalDiscountPercentage;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static EventPassTypeResponseDto? ToResponse(this EventPassType? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static EventPassTypeResponseDto? ToResponse(this EventPassType? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<EventPassTypeResponseDto>(source, out var existing)) return existing;
        var target = new EventPassTypeResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.ValidityPeriodInMonths = source.ValidityPeriodInMonths;
        target.RenewalDiscountPercentage = source.RenewalDiscountPercentage;
        target.Price = source.Price;
        target.Id = source.Id;
        return target;
    }
}
