using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Identity.DTO.RequestDto;
using EventFlowAPI.Logic.Identity.DTO.ResponseDto;
using EventFlowAPI.Logic.Identity.Helpers;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Identity.Services.Services.BaseServices;
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EventFlowAPI.Logic.Identity.Services.Services
{
    public class AuthService(
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        IJWTGeneratorService jwtGeneratorService) : BaseAuthService(userManager, httpContextAccessor, configuration, unitOfWork, jwtGeneratorService), IAuthService
    {
        public async Task<Result<User>> GetCurrentUserAsEntity()
        {
            var currentUserIdResult = GetCurrentUserId();
            if (!currentUserIdResult.IsSuccessful)
                return Result<User>.Failure(currentUserIdResult.Error);

            var user = await GetOneAsync(currentUserIdResult.Value);
            if (user is null)
                return Result<User>.Failure(UserError.UserNotFound);

            if (string.IsNullOrEmpty(user.Value.Email))
                return Result<User>.Failure(UserError.UserEmailNotFound);

            return Result<User>.Success(user.Value);
        }

        public async Task<Result<UserResponseDto>> GetCurrentUser()
        {
            var getUserResult = await GetCurrentUserAsEntity();
            if (!getUserResult.IsSuccessful) 
                return Result<UserResponseDto>.Failure(getUserResult.Error);

            var responseDto = MapAsDto(getUserResult.Value);

            return Result<UserResponseDto>.Success(responseDto);
        }
        private UserResponseDto MapAsDto(User entity)
        {
            var userDto = entity.AsDto<UserResponseDto>();
            userDto.EmailAddress = entity.Email!;
            userDto.UserData = entity.UserData?.AsDto<UserDataResponseDto>();
            userDto.PhotoEndpoint = $"/Users/info/image";
            userDto.UserRoles = entity.Roles.Select(r => r.Name).ToList();
            return userDto;
        }

        private async Task<Result<User>> GetOneAsync(string? id)
        {
            if (id == null || id == string.Empty)
                return Result<User>.Failure(Error.RouteParamOutOfRange);

            var _userRepository = (IUserRepository)_unitOfWork.GetRepository<User>();
            var record = await _userRepository.GetOneAsync(id);
            if (record == null)
                return Result<User>.Failure(Error.NotFound);

            return Result<User>.Success(record);
        }

        public async Task<Result<UserRegisterResponseDto>> RegisterUser(UserRegisterRequestDto? requestDto)
        {
            if(requestDto == null)
            {
                return Result<UserRegisterResponseDto>.Failure(Error.NullParameter);
            }

            var isUserWithSameEmailExistInDB = await _userManager.FindByEmailAsync(requestDto.Email);
            if(isUserWithSameEmailExistInDB != null)
            {
                return Result<UserRegisterResponseDto>.Failure(AuthError.EmailAlreadyTaken);
            }

            var user = requestDto.AsEntity<User>();
            user.RegisteredDate = DateTime.Now;

            var result = await _userManager.CreateAsync(user, requestDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.ToDictionary(e => e.Code, e => e.Description);
                return Result<UserRegisterResponseDto>.Failure(new AuthError(errors).ErrorsWhileCreatingUser);
            }

            await _userManager.AddToRoleAsync(user, Roles.User.ToString());

            return Result<UserRegisterResponseDto>.Success();
        }

        public async Task<Result<LoginResponseDto>> Login(LoginRequestDto? requestDto)
        {
            if (requestDto == null)
                return Result<LoginResponseDto>.Failure(Error.NullParameter);

            var user = await _userManager.FindByEmailAsync(requestDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, requestDto.Password))
                return Result<LoginResponseDto>.Failure(AuthError.InvalidEmailOrPassword);

            var roles = await GetRolesForCurrentUser(user);

            var responseDto = new LoginResponseDto
            {
                Token = _jwtGeneratorService.GenerateToken(user, roles),
            };

            return Result<LoginResponseDto>.Success(responseDto);
        }

    }
}
