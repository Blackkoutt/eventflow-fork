using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Mapper.Extensions;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using Serilog;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public class UserService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IFileService fileService) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;
        private readonly IFileService _fileService = fileService;

        public async Task<Result<IEnumerable<UserResponseDto>>> GetAllAsync(UserDataQuery query)
        {
            var records = await _unitOfWork.Users.GetAllAsync(query);
            var response = MapAsDto(records);
            return Result<IEnumerable<UserResponseDto>>.Success(response);
        }

        public async Task<Result<UserResponseDto>> GetOneAsync(string id)
        {
            var record = await _unitOfWork.Users.GetOneAsync(id);
            if (record == null)
                return Result<UserResponseDto>.Failure(Error.NotFound);

            var response = MapAsDto(record);

            return Result<UserResponseDto>.Success(response);
        }

        public async Task<Result<BlobResponseDto>> GetUserPhoto()
        {
            var userResult = await _authService.GetCurrentUserAsEntity();
            if (!userResult.IsSuccessful)
                return Result<BlobResponseDto>.Failure(userResult.Error);
            var user = userResult.Value;

            return await _fileService.GetEntityPhoto(user);
        }

        public async Task<Result<object>> SetUserInfo(UserDataRequestDto userDataRequestDto)
        {
            var userResult = await _authService.GetCurrentUserAsEntity();
            if (!userResult.IsSuccessful)
                return Result<object>.Failure(userResult.Error);
            var user = userResult.Value;

            var userData = userDataRequestDto.AsEntity<UserData>();
            user.UserData = userData;

            //Log.Information($"Hello {userDataRequestDto.UserPhoto}");
            var photoPostError = await _fileService.PostPhoto(user, userDataRequestDto.UserPhoto, $"user_{user.Id}", isUpdate: true);
            if (photoPostError != Error.None)
                return Result<object>.Failure(photoPostError);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Result<object>.Success();
        }

        private IEnumerable<UserResponseDto> MapAsDto(IEnumerable<User> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.AsDto<UserResponseDto>();
                responseDto.UserRoles = new List<string> { entity.Role };
                responseDto.EmailAddress = entity.Email!;
                responseDto.PhotoEndpoint = $"/Users/{responseDto.Id}/image";
                return responseDto;
            });
        }

        private UserResponseDto MapAsDto(User entity)
        {
            var responseDto = entity.AsDto<UserResponseDto>();
            responseDto.UserRoles = new List<string> { entity.Role };
            responseDto.EmailAddress = entity.Email!;
            responseDto.PhotoEndpoint = $"/Users/{responseDto.Id}/image";
            responseDto.AllReservationsCount = entity.Reservations.Count;
            responseDto.AllHallRentsCount = entity.HallRents.Count;
            responseDto.IsActiveEventPass = entity.EventPasses.Any(ep => !ep.IsDeleted &&
            !ep.IsExpired);
            return responseDto;
        }
    }
}
