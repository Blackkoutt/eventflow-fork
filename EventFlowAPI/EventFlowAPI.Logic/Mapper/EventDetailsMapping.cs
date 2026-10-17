using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.ResponseDto;

namespace EventFlowAPI.Logic.Mapper;

public static class EventDetailsMapping
{
    [return: NotNullIfNotNull(nameof(source))]
    public static EventDetailsResponseDto? ToResponse(this EventDetails? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static EventDetailsResponseDto? ToResponse(this EventDetails? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<EventDetailsResponseDto>(source, out var existing)) return existing;
        var target = new EventDetailsResponseDto();
        context.Add(source, target);
        target.LongDescription = source.LongDescription;
        target.Id = source.Id;
        return target;
    }
}
