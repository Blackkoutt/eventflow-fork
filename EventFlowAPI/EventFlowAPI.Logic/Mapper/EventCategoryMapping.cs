using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class EventCategoryMapping
{
    public static EventCategory ToEntity(this EventCategoryRequestDto source) => source is null ? null! : source.ApplyTo(new EventCategory());

    public static EventCategory ApplyTo(this EventCategoryRequestDto source, EventCategory target)
    {
        target.Name = source.Name;
        target.Icon = source.Icon;
        target.Color = source.Color;
        return target;
    }

    public static EventCategory ToEntity(this UpdateEventCategoryRequestDto source) => source is null ? null! : source.ApplyTo(new EventCategory());

    public static EventCategory ApplyTo(this UpdateEventCategoryRequestDto source, EventCategory target)
    {
        target.Name = source.Name;
        target.Icon = source.Icon;
        target.Color = source.Color;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static EventCategoryResponseDto? ToResponse(this EventCategory? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static EventCategoryResponseDto? ToResponse(this EventCategory? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<EventCategoryResponseDto>(source, out var existing)) return existing;
        var target = new EventCategoryResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.Icon = source.Icon;
        target.Color = source.Color;
        target.Id = source.Id;
        return target;
    }
}
