using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;

namespace EventFlowAPI.Logic.Mapper;

public static class SeatMapping
{
    public static Seat ToEntity(this SeatRequestDto source) => source is null ? null! : source.ApplyTo(new Seat());

    public static Seat ApplyTo(this SeatRequestDto source, Seat target)
    {
        target.SeatNr = source.SeatNr;
        target.Row = source.Row;
        target.GridRow = source.GridRow;
        target.Column = source.Column;
        target.GridColumn = source.GridColumn;
        target.SeatTypeId = source.SeatTypeId;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static SeatResponseDto? ToResponse(this Seat? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static SeatResponseDto? ToResponse(this Seat? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<SeatResponseDto>(source, out var existing)) return existing;
        var target = new SeatResponseDto();
        context.Add(source, target);
        target.SeatNr = source.SeatNr;
        target.Row = source.Row;
        target.GridRow = source.GridRow;
        target.Column = source.Column;
        target.GridColumn = source.GridColumn;
        target.SeatType = source.SeatType.ToResponse(context);
        target.Id = source.Id;
        return target;
    }
}
