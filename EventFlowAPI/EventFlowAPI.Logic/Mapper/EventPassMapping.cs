using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class EventPassMapping
{
    public static EventPass ToEntity(this EventPassRequestDto source) => source is null ? null! : source.ApplyTo(new EventPass());

    public static EventPass ApplyTo(this EventPassRequestDto source, EventPass target)
    {
        target.PassTypeId = source.PassTypeId;
        return target;
    }

    public static EventPass ToEntity(this UpdateEventPassRequestDto source) => source is null ? null! : source.ApplyTo(new EventPass());

    public static EventPass ApplyTo(this UpdateEventPassRequestDto source, EventPass target)
    {
        target.PassTypeId = source.PassTypeId;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static EventPassResponseDto? ToResponse(this EventPass? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static EventPassResponseDto? ToResponse(this EventPass? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<EventPassResponseDto>(source, out var existing)) return existing;
        var target = new EventPassResponseDto();
        context.Add(source, target);
        target.EventPassGuid = source.EventPassGuid;
        target.StartDate = source.StartDate;
        target.RenewalDate = source.RenewalDate;
        target.EndDate = source.EndDate;
        target.DeleteDate = source.DeleteDate;
        target.PaymentDate = source.PaymentDate;
        target.PaymentAmount = source.PaymentAmount;
        target.PassType = source.PassType.ToResponse(context);
        target.User = source.User.ToResponse(context);
        target.PaymentType = source.PaymentType.ToResponse(context);
        target.Id = source.Id;
        return target;
    }
}
