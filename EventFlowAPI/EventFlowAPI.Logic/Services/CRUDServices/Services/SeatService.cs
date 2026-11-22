using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class SeatService(IUnitOfWork unitOfWork) : ISeatService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public bool IsSeatHaveActiveReservationForEvent(Seat seatEntity, Event eventEntity)
        {
            foreach(var r in seatEntity.Reservations)
            {
                if (!r.IsExpired && !r.IsDeleted && r.Ticket?.Event.Id == eventEntity.Id)
                    return true;
            }
            return false;
        }         

        public async Task<IEnumerable<Seat>> GetSeatsByListOfIds(List<int> seatsIds)
        {
           return await _unitOfWork.Seats.GetByIdsAsync(seatsIds);
        }
    }
}
