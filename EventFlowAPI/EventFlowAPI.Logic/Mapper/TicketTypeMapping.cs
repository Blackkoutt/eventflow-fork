using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class TicketTypeMapping
{
    public static TicketType ToEntity(this TicketTypeRequestDto source) => source is null ? null! : source.ApplyTo(new TicketType());

    public static TicketType ApplyTo(this TicketTypeRequestDto source, TicketType target)
    {
        target.Name = source.Name;
        return target;
    }

    public static TicketType ToEntity(this UpdateTicketTypeRequestDto source) => source is null ? null! : source.ApplyTo(new TicketType());

    public static TicketType ApplyTo(this UpdateTicketTypeRequestDto source, TicketType target)
    {
        target.Name = source.Name;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static TicketTypeResponseDto? ToResponse(this TicketType? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static TicketTypeResponseDto? ToResponse(this TicketType? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<TicketTypeResponseDto>(source, out var existing)) return existing;
        var target = new TicketTypeResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.Id = source.Id;
        return target;
    }
}
