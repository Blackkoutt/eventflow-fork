using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Logic.Services.OtherServices.Services
{
    public class CopyMakerService(IUnitOfWork unitOfWork) : ICopyMakerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>Saves a hidden copy of a visible hall for a new event or hall rent.</summary>
        public async Task<Result<Hall>> MakeCopyOfHall(int hallId)
        {
            var hallEntity = await _unitOfWork.Halls.GetByIdAsync(hallId);

            if (hallEntity == null || !hallEntity.IsVisible)
                return Result<Hall>.Failure(HallError.NotFound);

            var hallCopy = await _unitOfWork.Halls.AddCopyAsync(hallEntity);
            await _unitOfWork.SaveChangesAsync();

            return Result<Hall>.Success(hallCopy);
        }
    }
}
