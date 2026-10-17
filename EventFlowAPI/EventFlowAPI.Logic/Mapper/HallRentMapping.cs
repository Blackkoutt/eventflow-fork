using EventFlowAPI.Logic.Extensions;
using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class HallRentMapping
{
    public static HallRent ToEntity(this HallRentRequestDto source) => source is null ? null! : source.ApplyTo(new HallRent());

    public static HallRent ApplyTo(this HallRentRequestDto source, HallRent target)
    {
        target.StartDate = source.StartDate;
        target.EndDate = source.EndDate;
        target.HallId = source.HallId;
        return target;
    }

    public static HallRent ToEntity(this UpdateHallRentRequestDto source) => source is null ? null! : source.ApplyTo(new HallRent());

    public static HallRent ApplyTo(this UpdateHallRentRequestDto source, HallRent target)
    {
        target.StartDate = source.StartDate;
        target.EndDate = source.EndDate;
        target.HallId = source.HallId;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static HallRentResponseDto? ToResponse(this HallRent? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static HallRentResponseDto? ToResponse(this HallRent? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<HallRentResponseDto>(source, out var existing)) return existing;
        var target = new HallRentResponseDto();
        context.Add(source, target);
        target.HallRentGuid = source.HallRentGuid;
        target.StartDate = source.StartDate;
        target.EndDate = source.EndDate;
        target.PaymentDate = source.RentDate;
        target.DeleteDate = source.DeleteDate;
        target.PaymentAmount = source.PaymentAmount;
        target.PaymentType = source.PaymentType.ToResponse();
        target.Hall = source.Hall.ToResponse(context);
        target.User = source.User.ToResponse(context);
        target.AdditionalServices = source.AdditionalServices?.Select(item => item.ToResponse(context)).ToList() ?? [];
        target.Id = source.Id;
        return target;
    }
}
