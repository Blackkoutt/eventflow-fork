using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class EventMapping
{
    public static Event ToEntity(this UpdateEventRequestDto source) => source is null ? null! : source.ApplyTo(new Event());

    public static Event ApplyTo(this UpdateEventRequestDto source, Event target)
    {
        target.Name = source.Name;
        target.ShortDescription = source.ShortDescription;
        target.StartDate = source.StartDate;
        target.EndDate = source.EndDate;
        target.CategoryId = source.CategoryId;
        target.HallId = source.HallId;
        return target;
    }

    public static Event ToEntity(this EventRequestDto source) => source is null ? null! : source.ApplyTo(new Event());

    public static Event ApplyTo(this EventRequestDto source, Event target)
    {
        target.Name = source.Name;
        target.ShortDescription = source.ShortDescription;
        target.StartDate = source.StartDate;
        target.EndDate = source.EndDate;
        target.CategoryId = source.CategoryId;
        target.HallId = source.HallId;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static EventResponseDto? ToResponse(this Event? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static EventResponseDto? ToResponse(this Event? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<EventResponseDto>(source, out var existing)) return existing;
        var target = new EventResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.ShortDescription = source.ShortDescription;
        target.StartDate = source.StartDate;
        target.EndDate = source.EndDate;
        target.Duration = source.Duration;
        target.PhotoName = source.PhotoName;
        target.Category = source.Category.ToResponse(context);
        target.LongDescription = source.LongDescription;
        target.Hall = source.Hall.ToResponse(context);
        target.Tickets = source.Tickets?.Select(item => item.ToResponse(context)).ToList() ?? [];
        target.Id = source.Id;
        return target;
    }
}
