using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class HallMapping
{
    public static Hall ToEntity(this HallRequestDto source) => source is null ? null! : source.ApplyTo(new Hall());

    public static Hall ApplyTo(this HallRequestDto source, Hall target)
    {
        target.HallNr = source.HallNr;
        target.RentalPricePerHour = source.RentalPricePerHour;
        target.Floor = source.Floor;
        target.HallTypeId = source.HallTypeId;
        source.HallDetails?.ApplyTo(target);
        target.Seats ??= [];
        target.Seats.Clear();
        foreach (var item in source.Seats?.Select(item => item.ToEntity()).ToList() ?? []) target.Seats.Add(item);
        return target;
    }

    public static Hall ToEntity(this UpdateHallRequestDto source) => source is null ? null! : source.ApplyTo(new Hall());

    public static Hall ApplyTo(this UpdateHallRequestDto source, Hall target)
    {
        target.HallNr = source.HallNr;
        target.RentalPricePerHour = source.RentalPricePerHour;
        target.HallTypeId = source.HallTypeId;
        target.Seats ??= [];
        target.Seats.Clear();
        foreach (var item in source.Seats?.Select(item => item.ToEntity()).ToList() ?? []) target.Seats.Add(item);
        return target;
    }

    public static Hall ToEntity(this EventHallRequestDto source) => source is null ? null! : source.ApplyTo(new Hall());

    public static Hall ApplyTo(this EventHallRequestDto source, Hall target)
    {
        target.Seats ??= [];
        target.Seats.Clear();
        foreach (var item in source.Seats?.Select(item => item.ToEntity()).ToList() ?? []) target.Seats.Add(item);
        return target;
    }

    public static Hall ToEntity(this HallRent_HallRequestDto source) => source is null ? null! : source.ApplyTo(new Hall());

    public static Hall ApplyTo(this HallRent_HallRequestDto source, Hall target)
    {
        target.Seats ??= [];
        target.Seats.Clear();
        foreach (var item in source.Seats?.Select(item => item.ToEntity()).ToList() ?? []) target.Seats.Add(item);
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static HallResponseDto? ToResponse(this Hall? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static HallResponseDto? ToResponse(this Hall? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<HallResponseDto>(source, out var existing)) return existing;
        var target = new HallResponseDto();
        context.Add(source, target);
        target.HallNr = source.HallNr;
        target.RentalPricePerHour = source.RentalPricePerHour;
        target.Floor = source.Floor;
        target.SeatsCount = source.Seats?.Count ?? 0;
        target.HallDetails = source.ToDetailsResponse();
        target.Type = source.Type.ToResponse(context);
        target.Seats = source.Seats?.Select(item => item.ToResponse(context)).ToList() ?? [];
        target.Id = source.Id;
        return target;
    }
}
