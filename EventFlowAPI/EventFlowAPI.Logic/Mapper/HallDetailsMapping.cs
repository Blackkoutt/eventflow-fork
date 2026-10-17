using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;

namespace EventFlowAPI.Logic.Mapper;

public static class HallDetailsMapping
{
    public static Hall ApplyTo(this HallDetailsRequestDto source, Hall target)
    {
        target.TotalLength = source.TotalLength;
        target.TotalWidth = source.TotalWidth;
        target.StageLength = source.StageLength;
        target.StageWidth = source.StageWidth;
        target.MaxNumberOfSeatsRows = source.MaxNumberOfSeatsRows;
        target.MaxNumberOfSeatsColumns = source.MaxNumberOfSeatsColumns;
        return target;
    }

    public static Hall ApplyTo(this EventHallDetailsRequestDto source, Hall target)
    {
        target.StageLength = source.StageLength;
        target.StageWidth = source.StageWidth;
        target.MaxNumberOfSeatsRows = source.MaxNumberOfSeatsRows;
        target.MaxNumberOfSeatsColumns = source.MaxNumberOfSeatsColumns;
        return target;
    }

    public static Hall ApplyTo(this HallRent_HallDetailsRequestDto source, Hall target)
    {
        target.StageLength = source.StageLength;
        target.StageWidth = source.StageWidth;
        target.MaxNumberOfSeatsRows = source.MaxNumberOfSeatsRows;
        target.MaxNumberOfSeatsColumns = source.MaxNumberOfSeatsColumns;
        return target;
    }

    public static HallDetailsResponseDto ToDetailsResponse(this Hall source) => new()
    {
        Id = source.Id,
        TotalLength = source.TotalLength,
        TotalWidth = source.TotalWidth,
        TotalArea = source.TotalArea,
        StageLength = source.StageLength,
        StageWidth = source.StageWidth,
        MaxNumberOfSeatsRows = source.MaxNumberOfSeatsRows,
        MaxNumberOfSeatsColumns = source.MaxNumberOfSeatsColumns,
        NumberOfSeats = source.NumberOfSeats,
        MaxNumberOfSeats = source.MaxNumberOfSeats
    };
}
