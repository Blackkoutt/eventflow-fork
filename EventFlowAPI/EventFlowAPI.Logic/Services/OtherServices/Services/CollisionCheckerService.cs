using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Logic.Services.OtherServices.Services
{
    public class CollisionCheckerService(IUnitOfWork unitOfWork) : ICollisionCheckerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<bool> CheckTimeCollisionsWithHallRents(int hallId, DateTime startDate, DateTime endDate, int? hallRentId = null)
        {
            return await _unitOfWork.HallRents.HasCollisionAsync(hallId, startDate, endDate, hallRentId);
        }
        public async Task<bool> CheckTimeCollisionsWithEvents(int hallId, DateTime startDate, DateTime endDate, int? eventId = null)
        {
            return await _unitOfWork.Events.HasCollisionAsync(hallId, startDate, endDate, eventId);
        }
    }
}
