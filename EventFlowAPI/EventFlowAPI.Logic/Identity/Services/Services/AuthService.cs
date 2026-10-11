using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Identity.DTO.RequestDto;
using EventFlowAPI.Logic.Identity.DTO.ResponseDto;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Identity.Services.Services.BaseServices;
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EventFlowAPI.Logic.Identity.Services.Services
{
    public class AuthService(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        IJWTGeneratorService jwtGeneratorService,
        IPasswordHasherService passwordHasher) : BaseAuthService(httpContextAccessor, configuration, unitOfWork, jwtGeneratorService), IAuthService
    {
        private readonly int _minPasswordLength = configuration.GetValue<int?>("Auth:MinPasswordLength")
            ?? throw new InvalidOperationException("Missing configuration value 'Auth:MinPasswordLength'.");
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;

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
            userDto.UserRoles = new List<string> { entity.Role.ToString() };
            return userDto;
        }

        private async Task<Result<User>> GetOneAsync(string? id)
        {
            if (id == null || id == string.Empty)
                return Result<User>.Failure(Error.RouteParamOutOfRange);

            var record = await _unitOfWork.Users.GetOneAsync(id);
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

            var isUserWithSameEmailExistInDB = await _unitOfWork.Users.FindByEmailAsync(requestDto.Email);
            if(isUserWithSameEmailExistInDB != null)
            {
                return Result<UserRegisterResponseDto>.Failure(AuthError.EmailAlreadyTaken);
            }

            if (requestDto.Password.Length < _minPasswordLength)
            {
                var errors = new Dictionary<string, string>
                {
                    { "PasswordTooShort", $"Passwords must be at least {_minPasswordLength} characters." }
                };
                return Result<UserRegisterResponseDto>.Failure(new AuthError(errors).ErrorsWhileCreatingUser);
            }

            var user = requestDto.AsEntity<User>();
            user.RegisteredDate = DateTime.Now;
            user.Role = Roles.User;
            user.PasswordHash = _passwordHasher.Hash(user, requestDto.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return Result<UserRegisterResponseDto>.Success();
        }

        public async Task<Result<LoginResponseDto>> Login(LoginRequestDto? requestDto)
        {
            if (requestDto == null)
                return Result<LoginResponseDto>.Failure(Error.NullParameter);

            var user = await _unitOfWork.Users.FindByEmailAsync(requestDto.Email);
            if (user == null || !_passwordHasher.Verify(user, requestDto.Password))
                return Result<LoginResponseDto>.Failure(AuthError.InvalidEmailOrPassword);

            var roles = GetRolesForCurrentUser(user);

            var responseDto = new LoginResponseDto
            {
                Token = _jwtGeneratorService.GenerateToken(user, roles),
            };

            return Result<LoginResponseDto>.Success(responseDto);
        }

    }
}
