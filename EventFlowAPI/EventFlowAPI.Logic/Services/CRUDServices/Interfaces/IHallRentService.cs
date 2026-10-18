using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;

namespace EventFlowAPI.Logic.Services.CRUDServices.Interfaces
{
    public interface IHallRentService
    {
        Task<Result<HallRentResponseDto>> AddAsync(HallRentRequestDto? requestDto);
        Task<Result<IEnumerable<HallRentResponseDto>>> GetAllAsync(HallRentQuery query);
        Task<Result<HallRentResponseDto>> GetOneAsync(int id);
        Task<Result<HallRentResponseDto>> UpdateAsync(int id, UpdateHallRentRequestDto? requestDto);
        Task<Result<object>> DeleteAsync(int id);
        Task<Result<HallRentResponseDto>> MakeRent(HallRentRequestDto? requestDto);
        Task<Error> SoftDeleteHallRent(HallRent hallRent);
    }
}
