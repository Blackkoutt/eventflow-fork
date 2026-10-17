using System.Diagnostics.CodeAnalysis;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;

namespace EventFlowAPI.Logic.Mapper;

public static class ReservationMapping
{
    public static Reservation ToEntity(this ReservationRequestDto source) => source is null ? null! : source.ApplyTo(new Reservation());

    public static Reservation ApplyTo(this ReservationRequestDto source, Reservation target)
    {
        target.PaymentTypeId = source.PaymentTypeId;
        target.TicketId = source.TicketId;
        return target;
    }

    public static Reservation ToEntity(this UpdateReservationRequestDto source) => source is null ? null! : source.ApplyTo(new Reservation());

    public static Reservation ApplyTo(this UpdateReservationRequestDto source, Reservation target)
    {
        target.PaymentTypeId = source.PaymentTypeId;
        target.TicketId = source.TicketId;
        return target;
    }

    [return: NotNullIfNotNull(nameof(source))]
    public static ReservationResponseDto? ToResponse(this Reservation? source) => source.ToResponse(new ResponseMappingContext());

    [return: NotNullIfNotNull(nameof(source))]
    internal static ReservationResponseDto? ToResponse(this Reservation? source, ResponseMappingContext context)
    {
        if (source is null) return null;
        if (context.TryGet<ReservationResponseDto>(source, out var existing)) return existing;
        var target = new ReservationResponseDto();
        context.Add(source, target);
        target.ReservationGuid = source.ReservationGuid;
        target.ReservationDate = source.ReservationDate;
        target.PaymentDate = source.PaymentDate;
        target.DeleteDate = source.DeleteDate;
        target.PaymentAmount = source.PaymentAmount;
        target.User = source.User.ToResponse(context);
        target.PaymentType = source.PaymentType.ToResponse(context);
        target.Ticket = source.Ticket.ToResponse(context);
        target.Seats = source.Seats?.Select(item => item.ToResponse(context)).ToList() ?? [];
        target.Id = source.Id;
        return target;
    }
}
