using System.Text.Json;
using System.Text.Json.Serialization;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Mapper;

namespace EventFlowAPI.Tests.Mapping;

public class MappingTests
{
    [Fact]
    public void Unified_partners_keep_kind_and_festival_response_groups()
    {
        var partners = Enum.GetValues<PartnerKind>()
            .Select(kind => new PartnerRequestDto { Kind = kind, Name = kind.ToString() }.ToEntity())
            .ToList();
        var response = new Festival { Partners = partners }.ToResponse();

        Assert.Equal(PartnerKind.MediaPatron, Assert.Single(response.MediaPatrons).Kind);
        Assert.Equal(PartnerKind.Organizer, Assert.Single(response.Organizers).Kind);
        Assert.Equal(PartnerKind.Sponsor, Assert.Single(response.Sponsors).Kind);
        var sponsor = partners.Single(p => p.Kind == PartnerKind.Sponsor);
        new UpdatePartnerRequestDto { Name = "Updated" }.ApplyTo(sponsor);
        Assert.Equal(PartnerKind.Sponsor, sponsor.Kind);
    }

    [Fact]
    public void Payment_enum_and_creation_dates_populate_response_contract()
    {
        var created = new DateTime(2030, 1, 2);
        var reservation = new Reservation { ReservationDate = created, PaymentType = PaymentType.PayU }.ToResponse();
        var rent = new HallRent { RentDate = created, PaymentType = PaymentType.Pass }.ToResponse();

        Assert.Equal(created, reservation.PaymentDate);
        Assert.NotNull(reservation.PaymentType);
        Assert.Equal((int)PaymentType.PayU, reservation.PaymentType.Id);
        Assert.Equal("PayU", reservation.PaymentType.Name);
        Assert.Equal(created, rent.PaymentDate);
        Assert.NotNull(rent.PaymentType);
        Assert.Equal((int)PaymentType.Pass, rent.PaymentType.Id);
        Assert.Equal("Karnet", rent.PaymentType.Name);
    }

    [Fact]
    public void Cyclic_event_and_festival_graph_preserves_references_and_serializes()
    {
        var entity = new Event { Id = 1 };
        var festival = new Festival { Id = 2, Events = [entity] };
        var ticket = new Ticket { Id = 3, Event = entity, Festival = festival };
        entity.Tickets.Add(ticket);
        festival.Tickets.Add(ticket);

        var response = entity.ToResponse();

        var mappedTicket = Assert.Single(response.Tickets);
        Assert.Same(response, mappedTicket.Event);
        Assert.Same(response, Assert.Single(mappedTicket.Festival!.Events));
        Assert.Same(mappedTicket, Assert.Single(mappedTicket.Festival.Tickets));
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        });
        using var document = JsonDocument.Parse(json);
        Assert.Equal(JsonValueKind.Null, document.RootElement.GetProperty("Tickets")[0].GetProperty("Event").ValueKind);
    }

    [Fact]
    public void Separate_mapping_calls_do_not_share_mutable_responses()
    {
        var entity = new Event { Id = 1, Hall = new Hall { Id = 2, Seats = [new Seat()] } };
        var first = entity.ToResponse();
        first.Hall!.Seats.Clear();

        var second = entity.ToResponse();

        Assert.NotSame(first, second);
        Assert.Single(second.Hall!.Seats);
        Assert.Single(entity.Hall.Seats);
        Assert.Equal(1, second.Hall.SeatsCount);
    }

    [Fact]
    public void Missing_navigation_properties_stay_null_and_null_collections_become_empty()
    {
        var entity = new Event { Hall = null!, Tickets = null! };

        var response = entity.ToResponse();

        Assert.Null(response.Hall);
        Assert.Null(response.Category);
        Assert.Null(response.Details);
        Assert.Empty(response.Tickets);
        Assert.Null(((Event?)null).ToResponse());
    }

    [Fact]
    public void Seat_update_preserves_tracked_identity_and_reservations()
    {
        var reservation = new Reservation { Id = 12 };
        var hall = new Hall { Id = 13 };
        var seat = new Seat { Id = 11, HallId = hall.Id, Hall = hall, Reservations = [reservation] };
        var reservations = seat.Reservations;

        var updated = new SeatRequestDto { SeatNr = 5, SeatTypeId = 7 }.ApplyTo(seat);

        Assert.Same(seat, updated);
        Assert.Equal(11, updated.Id);
        Assert.Equal(13, updated.HallId);
        Assert.Same(hall, updated.Hall);
        Assert.Same(reservations, updated.Reservations);
        Assert.Same(reservation, Assert.Single(updated.Reservations));
        Assert.Equal(5, updated.SeatNr);
        Assert.Equal(7, updated.SeatTypeId);
    }

    [Fact]
    public void Hall_details_update_retains_identity_and_recalculates_area()
    {
        var hall = new Hall { Id = 10, TotalLength = 5, TotalWidth = 6, NumberOfSeats = 4 };
        var request = new HallRequestDto
        {
            HallDetails = new HallDetailsRequestDto { TotalLength = 6, TotalWidth = 7 }
        };

        request.ApplyTo(hall);

        Assert.Equal(10, hall.Id);
        Assert.Equal(42, hall.TotalArea);
        Assert.Equal(4, hall.NumberOfSeats);
        Assert.Equal(6, hall.TotalLength);
        Assert.Equal(7, hall.TotalWidth);
    }

    [Fact]
    public void Create_from_update_has_fresh_identity_and_relationships()
    {
        var previous = new SeatType { Id = 8, Name = "Previous", IsDeleted = true };
        var request = new UpdateSeatTypeRequestDto { Name = "Replacement", SeatColor = "#123456" };

        var replacement = request.ToEntity();

        Assert.Equal(0, replacement.Id);
        Assert.False(replacement.IsDeleted);
        Assert.Empty(replacement.Seats);
        Assert.Equal("Replacement", replacement.Name);
        Assert.Equal("Previous", previous.Name);
        Assert.Equal(8, previous.Id);
    }

    [Fact]
    public void Partial_update_clears_nullable_description_without_overwriting_name_or_identity()
    {
        var entity = new Equipment { Id = 9, Name = "Original", Description = "Old" };

        new UpdateEquipmentRequestDto { Description = null }.ApplyTo(entity);

        Assert.Null(entity.Description);
        Assert.Equal("Original", entity.Name);
        Assert.Equal(9, entity.Id);
    }

    [Fact]
    public void Duration_uses_dates_and_existing_user_data_conversions_remain_compatible()
    {
        var start = new DateTime(2030, 1, 1);
        Assert.Equal(TimeSpan.FromHours(1), new Event { StartDate = start, EndDate = start.AddHours(1) }.ToResponse().Duration);
        Assert.Equal(TimeSpan.FromHours(2), new Festival { StartDate = start, EndDate = start.AddHours(2) }.ToResponse().Duration);

        var response = new User { Id = "42", Street = "Wiejska", HouseNumber = null }.ToUserDataResponse();
        Assert.Equal(42, response!.Id);
        Assert.Equal(0, response.HouseNumber);
        Assert.Equal(0, new User { Id = "non-numeric", City = "Poznań" }.ToUserDataResponse()!.Id);
        Assert.Null(new User { Id = "42" }.ToUserDataResponse());
    }
}
