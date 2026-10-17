using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface ISeatService /*:
        IGenericService<
            Seat,
            SeatRequestDto,
            SeatResponseDto
        >*/
    {
        bool IsSeatHaveActiveReservationForEvent(Seat seatEntity, Event eventEntity);
        Task<IEnumerable<Seat>> GetSeatsByListOfIds(List<int> seatsIds);
    }
}
