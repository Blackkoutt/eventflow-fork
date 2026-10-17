using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;

namespace EventFlowAPI.Logic.Mapper;

public static class TicketMapping
{
    public static Ticket ToEntity(this TicketRequestDto source) => source is null ? null! : source.ApplyTo(new Ticket());

    public static Ticket ApplyTo(this TicketRequestDto source, Ticket target)
    {
        target.Price = source.Price;
        target.TicketTypeId = source.TicketTypeId;
        target.EventId = source.EventId;
        target.FestivalId = source.FestivalId;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static TicketResponseDto? ToResponse(this Ticket? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static TicketResponseDto? ToResponse(this Ticket? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<TicketResponseDto>(source, out var existing)) return existing;
        var target = new TicketResponseDto();
        context.Add(source, target);
        target.Price = source.Price;
        target.Event = source.Event.ToResponse(context);
        target.Festival = source.Festival.ToResponse(context);
        target.TicketType = source.TicketType.ToResponse(context);
        target.Id = source.Id;
        return target;
    }
}
