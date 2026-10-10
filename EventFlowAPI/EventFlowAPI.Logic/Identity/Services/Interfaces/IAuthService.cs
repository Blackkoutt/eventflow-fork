using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Identity.DTO.RequestDto;
using EventFlowAPI.Logic.Identity.DTO.ResponseDto;
using EventFlowAPI.Logic.Identity.Services.Interfaces.BaseInterfaces;
using EventFlowAPI.Logic.ResultObject;

namespace EventFlowAPI.Logic.Identity.Services.Interfaces
{
    public interface IAuthService : IBaseAuthService
    {
        Task<Result<User>> GetCurrentUserAsEntity();
        Task<Result<UserResponseDto>> GetCurrentUser();
        Task<Result<UserRegisterResponseDto>> RegisterUser(UserRegisterRequestDto? requestDto);
        Task<Result<LoginResponseDto>> Login(LoginRequestDto? requestDto);
    }
}
