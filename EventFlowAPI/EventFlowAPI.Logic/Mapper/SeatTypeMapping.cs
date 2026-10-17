using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class SeatTypeMapping
{
    public static SeatType ToEntity(this SeatTypeRequestDto source) => source is null ? null! : source.ApplyTo(new SeatType());

    public static SeatType ApplyTo(this SeatTypeRequestDto source, SeatType target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        target.AddtionalPaymentPercentage = source.AddtionalPaymentPercentage;
        target.SeatColor = source.SeatColor;
        return target;
    }

    public static SeatType ToEntity(this UpdateSeatTypeRequestDto source) => source is null ? null! : source.ApplyTo(new SeatType());

    public static SeatType ApplyTo(this UpdateSeatTypeRequestDto source, SeatType target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        target.AddtionalPaymentPercentage = source.AddtionalPaymentPercentage;
        target.SeatColor = source.SeatColor;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static SeatTypeResponseDto? ToResponse(this SeatType? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static SeatTypeResponseDto? ToResponse(this SeatType? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<SeatTypeResponseDto>(source, out var existing)) return existing;
        var target = new SeatTypeResponseDto();
        context.Add(source, target);
        target.Name = source.Name;
        target.Description = source.Description;
        target.SeatColor = source.SeatColor;
        target.AddtionalPaymentPercentage = source.AddtionalPaymentPercentage;
        target.Id = source.Id;
        return target;
    }
}
