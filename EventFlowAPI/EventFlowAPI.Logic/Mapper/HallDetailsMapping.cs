using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;

namespace EventFlowAPI.Logic.Mapper;

public static class HallDetailsMapping
{
    public static HallDetails ToEntity(this HallDetailsRequestDto source) => source is null ? null! : source.ApplyTo(new HallDetails());

    public static HallDetails ApplyTo(this HallDetailsRequestDto source, HallDetails target)
    {
        target.TotalLength = source.TotalLength;
        target.TotalWidth = source.TotalWidth;
        target.StageLength = source.StageLength;
        target.StageWidth = source.StageWidth;
        target.MaxNumberOfSeatsRows = source.MaxNumberOfSeatsRows;
        target.MaxNumberOfSeatsColumns = source.MaxNumberOfSeatsColumns;
        return target;
    }

    public static HallDetails ToEntity(this EventHallDetailsRequestDto source) => source is null ? null! : source.ApplyTo(new HallDetails());

    public static HallDetails ApplyTo(this EventHallDetailsRequestDto source, HallDetails target)
    {
        target.StageLength = source.StageLength;
        target.StageWidth = source.StageWidth;
        target.MaxNumberOfSeatsRows = source.MaxNumberOfSeatsRows;
        target.MaxNumberOfSeatsColumns = source.MaxNumberOfSeatsColumns;
        return target;
    }

    public static HallDetails ToEntity(this HallRent_HallDetailsRequestDto source) => source is null ? null! : source.ApplyTo(new HallDetails());

    public static HallDetails ApplyTo(this HallRent_HallDetailsRequestDto source, HallDetails target)
    {
        target.StageLength = source.StageLength;
        target.StageWidth = source.StageWidth;
        target.MaxNumberOfSeatsRows = source.MaxNumberOfSeatsRows;
        target.MaxNumberOfSeatsColumns = source.MaxNumberOfSeatsColumns;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static HallDetailsResponseDto? ToResponse(this HallDetails? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static HallDetailsResponseDto? ToResponse(this HallDetails? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<HallDetailsResponseDto>(source, out var existing)) return existing;
        var target = new HallDetailsResponseDto();
        context.Add(source, target);
        target.TotalLength = source.TotalLength;
        target.TotalWidth = source.TotalWidth;
        target.TotalArea = source.TotalArea;
        target.StageLength = source.StageLength;
        target.StageWidth = source.StageWidth;
        target.MaxNumberOfSeatsRows = source.MaxNumberOfSeatsRows;
        target.MaxNumberOfSeatsColumns = source.MaxNumberOfSeatsColumns;
        target.NumberOfSeats = source.NumberOfSeats;
        target.MaxNumberOfSeats = source.MaxNumberOfSeats;
        target.Id = source.Id;
        return target;
    }
}
