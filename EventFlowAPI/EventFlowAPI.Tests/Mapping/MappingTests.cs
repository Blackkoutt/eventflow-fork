using System.Text.Json;
using System.Text.Json.Serialization;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Mapper;

namespace EventFlowAPI.Tests.Mapping;

public class MappingTests
{
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
    public void Nested_hall_details_update_retains_identity_and_service_calculated_values()
    {
        var details = new HallDetails { Id = 20, TotalArea = 30, NumberOfSeats = 4 };
        var hall = new Hall { Id = 10, HallDetails = details };
        var request = new HallRequestDto
        {
            HallDetails = new HallDetailsRequestDto { TotalLength = 6, TotalWidth = 7 }
        };

        request.ApplyTo(hall);

        Assert.Same(details, hall.HallDetails);
        Assert.Equal(20, details.Id);
        Assert.Equal(30, details.TotalArea);
        Assert.Equal(4, details.NumberOfSeats);
        Assert.Equal(6, details.TotalLength);
        Assert.Equal(7, details.TotalWidth);
    }

    [Fact]
    public void Soft_update_creates_fresh_entity_without_mutating_previous_version()
    {
        var previous = new SeatType { Id = 8, Name = "Previous", IsDeleted = true, IsSoftUpdated = true };
        var request = new UpdateSeatTypeRequestDto { Name = "Replacement", SeatColor = "#123456" };

        var replacement = request.ToEntity();

        Assert.Equal(0, replacement.Id);
        Assert.False(replacement.IsDeleted);
        Assert.False(replacement.IsSoftUpdated);
        Assert.Empty(replacement.Seats);
        Assert.Equal("Replacement", replacement.Name);
        Assert.Equal("Previous", previous.Name);
        Assert.Equal(8, previous.Id);
    }

    [Fact]
    public void Partial_update_clears_nullable_description_without_overwriting_name_or_flags()
    {
        var entity = new Equipment { Id = 9, Name = "Original", Description = "Old", IsUpdated = true };

        new UpdateEquipmentRequestDto { Description = null }.ApplyTo(entity);

        Assert.Null(entity.Description);
        Assert.Equal("Original", entity.Name);
        Assert.Equal(9, entity.Id);
        Assert.True(entity.IsUpdated);
    }

    [Fact]
    public void Existing_duration_and_user_data_conversions_remain_compatible()
    {
        // Characterize existing API quirks; changing these belongs to a separate behavior change.
        Assert.Equal(TimeSpan.FromTicks(3600), new Event { Duration = 3600 }.ToResponse().Duration);
        Assert.Equal(TimeSpan.FromTicks(3600), new Festival { Duration = 3600 }.ToResponse().Duration);

        var response = new UserData { Id = "42", HouseNumber = null }.ToResponse();
        Assert.Equal(42, response.Id);
        Assert.Equal(0, response.HouseNumber);
        Assert.Throws<FormatException>(() => new UserData { Id = "non-numeric" }.ToResponse());
    }
}
