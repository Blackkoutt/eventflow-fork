using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.Logic.UnitOfWork;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

using EventFlowAPI.Logic.Services.OtherServices.Services;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Serilog;
using EventFlowAPI.Logic.Enums;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class HallRentService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        ICopyMakerService copyMaker,
        ICollisionCheckerService collisionChecker,
        IFileService fileService,
        IHttpContextAccessor httpContextAccessor) :
        GenericService<
            HallRent,
            HallRentRequestDto,
            UpdateHallRentRequestDto,
            HallRentResponseDto,
            HallRentQuery
        >(unitOfWork, authService),
        IHallRentService
    {
        private readonly ICopyMakerService _copyMaker = copyMaker;
        private readonly ICollisionCheckerService _collisionChecker = collisionChecker;
        private readonly IFileService _fileService = fileService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public sealed override async Task<Result<IEnumerable<HallRentResponseDto>>> GetAllAsync(HallRentQuery query)
        {
            UserResponseDto? user = default!;
            if(query.GetAll != true)
            {
                var userResult = await _authService.GetCurrentUser();
                if (!userResult.IsSuccessful)
                    return Result<IEnumerable<HallRentResponseDto>>.Failure(userResult.Error);
                user = userResult.Value;
            }     

            if (query.GetAll == true || user.IsInRole(Roles.Admin))
            {
                var allHallRents = await _repository.GetAllAsync(q => q.ByQuery(query)
                                                                       .GetPage(query.PageNumber, query.PageSize));

                var allHallRentsDto = MapAsDto(allHallRents);
                return Result<IEnumerable<HallRentResponseDto>>.Success(allHallRentsDto);
            }
            else if (user.IsInRole(Roles.User))
            {
                var userHallRents = await _repository.GetAllAsync(q =>
                                            q.ByStatus(query.Status)
                                            .Where(hr => hr.User.Id == user.Id)
                                            .SortBy(query.SortBy, query.SortDirection)
                                            .GetPage(query.PageNumber, query.PageSize));

                var userHallRentsResponse = MapAsDto(userHallRents);
                return Result<IEnumerable<HallRentResponseDto>>.Success(userHallRentsResponse);
            }
            else
            {
                return Result<IEnumerable<HallRentResponseDto>>.Failure(AuthError.UserDoesNotHaveSpecificRole);
            }
        }

        public sealed override async Task<Result<HallRentResponseDto>> GetOneAsync(int id)
        {
            var hallRentResult = await base.GetOneAsync(id);
            if (!hallRentResult.IsSuccessful)
                return Result<HallRentResponseDto>.Failure(hallRentResult.Error);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<HallRentResponseDto>.Failure(userResult.Error);

            var user = userResult.Value;
            var hallRent = hallRentResult.Value;
            var premissionError = CheckUserPremission(user, hallRent.User!.Id);
            if (premissionError != Error.None)
                return Result<HallRentResponseDto>.Failure(premissionError);

            return Result<HallRentResponseDto>.Success(hallRent);
        }

        public async Task<Result<HallRentResponseDto>> MakeRent(HallRentRequestDto? requestDto)
        {
            // Validation
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<HallRentResponseDto>.Failure(validationError);

            // User
            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<HallRentResponseDto>.Failure(userResult.Error);
            var user = userResult.Value;

            // HallRent Entity
            var hallRentCreateResult = await CreateHallRentEntity(requestDto!, user.Id);
            if(!hallRentCreateResult.IsSuccessful)
                return Result<HallRentResponseDto>.Failure(hallRentCreateResult.Error);
            var hallRent = hallRentCreateResult.Value;
            await _repository.AddAsync(hallRent);
            await _unitOfWork.SaveChangesAsync();

            // HallView PDF
            var hallViewFileNameResult = await _fileService.CreateHallViewPDF(hallRent.Hall, hallRent, null, isUpdate: false);
            if (!hallViewFileNameResult.IsSuccessful)
                return Result<HallRentResponseDto>.Failure(hallViewFileNameResult.Error); 
            var hallViewPDFFileName = hallViewFileNameResult.Value;    
            hallRent.Hall.HallViewFileName = hallViewPDFFileName;

            // Hall Rent PDF
            var hallRentFileNameResult = await _fileService.CreateHallRentPDF(hallRent, isUpdate: false);
            if (!hallRentFileNameResult.IsSuccessful)
                return Result<HallRentResponseDto>.Failure(hallRentFileNameResult.Error);
            var hallRentPDFFileName = hallRentFileNameResult.Value.FileName;
            hallRent.HallRentPDFName = hallRentPDFFileName;

            _repository.Update(hallRent);
            await _unitOfWork.SaveChangesAsync();

            var response = MapAsDto(hallRent);

            return Result<HallRentResponseDto>.Success(response);
        }

        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var hallRent = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            var deleteError = await SoftDeleteHallRent(hallRent);
            if (deleteError != Error.None)
                return Result<object>.Failure(deleteError);

            await _unitOfWork.SaveChangesAsync();

            return Result<object>.Success();
        }

        public async Task<Error> UpdateHallRentsFiles(IEnumerable<HallRent> activeHallRents)
        {
            foreach (var hallRent in activeHallRents)
            {
                var hallViewFileNameResult = await _fileService.CreateHallViewPDF(hallRent.Hall, hallRent, null, isUpdate: true);
                if (!hallViewFileNameResult.IsSuccessful)
                {
                    return hallViewFileNameResult.Error;
                }

                var hallRentFileNameResult = await _fileService.CreateHallRentPDF(hallRent, isUpdate: true);
                if (!hallRentFileNameResult.IsSuccessful)
                {
                    return hallRentFileNameResult.Error;
                }

                hallRent.Hall.HallViewFileName = hallViewFileNameResult.Value;
                hallRent.HallRentPDFName = hallRentFileNameResult.Value.FileName;
                _unitOfWork.GetRepository<HallRent>().Update(hallRent);
            }
            await _unitOfWork.SaveChangesAsync();

            return Error.None;
        }

        public async Task<Error> SoftDeleteHallRent(HallRent hallRent)
        {
            // Soft Delete HallRent
            hallRent.DeleteDate = DateTime.Now;
            hallRent.IsDeleted = true;

            // Delete Hall Rent PDF in Blob Storage
            var hallRentPDFDeleteError = await _fileService.DeleteFile(hallRent, FileType.PDF, BlobContainer.HallRentsPDF);
            if (hallRentPDFDeleteError != Error.None)
                return hallRentPDFDeleteError;
            hallRent.HallRentPDFName = null;

            // Hard Delete Seats
            foreach (var seat in hallRent.Hall.Seats)
            {
                _unitOfWork.GetRepository<Seat>().Delete(seat);
            }

            // Delete Hall View PDF in Blob Storage
            var hallViewPDFDeleteError = await _fileService.DeleteFile(hallRent.Hall, FileType.PDF, BlobContainer.HallViewsPDF);
            if (hallViewPDFDeleteError != Error.None)
                return hallViewPDFDeleteError;
            hallRent.Hall.HallViewFileName = null;

            _repository.Update(hallRent);

            return Error.None;
        }


        private async Task<Result<HallRent>> CreateHallRentEntity(HallRentRequestDto requestDto, string userId)
        {
            var additionalServices = await _unitOfWork.GetRepository<AdditionalServices>()
                                      .GetAllAsync(q => q.Where(s => !s.IsDeleted && requestDto.AdditionalServicesIds.Contains(s.Id)));

            var hallResult = await _copyMaker.MakeCopyOfHall(requestDto.HallId);
            if (!hallResult.IsSuccessful)
                return Result<HallRent>.Failure(hallResult.Error);

            var hall = hallResult.Value;

            var hallRent = new HallRent
            {
                HallRentGuid = Guid.NewGuid(),
                StartDate = requestDto.StartDate,
                EndDate = requestDto.EndDate,
                RentDate = DateTime.Now,
                PaymentAmount = CalculatePaymentAmount(requestDto, hall, additionalServices),
                PaymentType = PaymentType.PayU,
                HallId = hall.Id,
                UserId = userId,
                AdditionalServices = additionalServices.ToList()
            };

            return Result<HallRent>.Success(hallRent);
        }
        private decimal CalculatePaymentAmount(HallRentRequestDto requestDto, Hall hall, IEnumerable<AdditionalServices> additionalServices)
        {
            var rentDuration = requestDto.EndDate - requestDto.StartDate;
            var totalHoursRent = (int)Math.Ceiling(rentDuration.TotalHours);
            var rentCost = Math.Round(hall.RentalPricePerHour * totalHoursRent, 2);
            var totalPayment = rentCost;
            foreach (var additionalService in additionalServices)
            {
                totalPayment += additionalService.Price;
            }
            return totalPayment;
        }

        protected sealed override IEnumerable<HallRentResponseDto> MapAsDto(IEnumerable<HallRent> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.ToResponse();
                responseDto.User = entity.User.ToResponse();
                responseDto.User.EmailAddress = entity.User.Email!;
                responseDto.User.UserData = null;
                responseDto.PaymentType = entity.PaymentType.ToResponse();
                responseDto.HallRentStatus = GetEntityStatus(entity);
                responseDto.Hall = entity.Hall.ToResponse();
                responseDto.Hall.Type = entity.Hall.Type.ToResponse();
                responseDto.Hall.Type.Equipments = [];
                responseDto.Hall.Seats = [];
                responseDto.Hall.HallDetails = null;
                responseDto.AdditionalServices = entity.AdditionalServices
                    .Select(services => services.ToResponse()).ToList();
                return responseDto;
            });
        }


        protected sealed override HallRentResponseDto MapAsDto(HallRent entity)
        {
            var responseDto = entity.ToResponse();
            responseDto.User = entity.User.ToResponse();
            responseDto.User.EmailAddress = entity.User.Email!;
            responseDto.User.UserData = null;
            responseDto.PaymentType = entity.PaymentType.ToResponse();
            responseDto.Hall = entity.Hall.ToResponse();
            responseDto.Hall.Type = entity.Hall.Type.ToResponse();
            responseDto.HallRentStatus = GetEntityStatus(entity);
            responseDto.Hall.Type.Equipments = [];
            responseDto.Hall.Seats = [];
            responseDto.Hall.HallDetails = null;
            responseDto.AdditionalServices = entity.AdditionalServices
                .Select(services => services.ToResponse()).ToList();
            return responseDto;
        }

        protected sealed override async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            if (id != null && id < 0)
                return Error.RouteParamOutOfRange;

            if (requestDto == null)
                return Error.NullParameter;

            int paymentTypeId;
            int hallId;
            List<int> additionalServicesIds = [];
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            switch (requestDto)
            {
                case HallRentRequestDto hallRentRequestDto:
                    paymentTypeId = 1;
                    hallId = hallRentRequestDto.HallId;
                    additionalServicesIds = hallRentRequestDto.AdditionalServicesIds;
                    startDate = hallRentRequestDto.StartDate;
                    break;
                case UpdateHallRentRequestDto updateHallRentRequestDto:
                    paymentTypeId = 1;
                    hallId = updateHallRentRequestDto.HallId;
                    additionalServicesIds = updateHallRentRequestDto.AdditionalServicesIds;
                    endDate = updateHallRentRequestDto.EndDate;
                    break;
                default:
                    return Error.BadRequestType;
            }

            // nie mozna robić update hall rentu
            if (id != null)
            {
                var hallRentId = id ?? -1;
                var hallRent = await _repository.GetOneAsync(hallRentId);
                if (hallRent == null)
                    return Error.NotFound;

                if (hallRent.IsExpired)
                    return HallRentError.HallRentIsExpired;

                if (hallRent.IsDeleted)
                    return HallRentError.HallRentIsDeleted;
            }

            if (!Enum.IsDefined(typeof(PaymentType), paymentTypeId))
                return PaymentTypeError.PaymentTypeNotFound;


            var hallEntity = await _unitOfWork.GetRepository<Hall>()
                               .GetAllAsync(q => q.Where(h =>
                               h.Id == hallId));
            if (hallEntity == null || !hallEntity.Any())
                return HallRentError.HallNotFound;

            if (additionalServicesIds.Distinct().Count() != additionalServicesIds.Count())
                return AdditionalServicesError.ServiceDuplicate;

            var additionalServices = await _unitOfWork.GetRepository<AdditionalServices>()
                                        .GetAllAsync(q => q.Where(s => !s.IsDeleted && additionalServicesIds.Contains(s.Id)));

            if (additionalServices.Count() < additionalServicesIds.Count)
                return AdditionalServicesError.ServiceNotFound;

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return userResult.Error;
            var user = userResult.Value;

            if (user.IsInRole(Roles.User))
            {
                var userActiveHallRentsInMonthCount = (await _repository.GetAllAsync(q =>
                                                            q.Where(hr =>
                                                                hr.User.Id == user.Id &&    
                                                                !hr.IsDeleted &&
                                                                hr.EndDate > DateTime.Now &&
                                                                hr.StartDate.Month == DateTime.Now.Month)))
                                                                .Count();
                var userMaxHallRentsInMonth = 5;
                if (userActiveHallRentsInMonthCount > userMaxHallRentsInMonth)
                    return HallRentError.TooMuchActiveHallRentsInMonth;
            }

            if (await _collisionChecker.CheckTimeCollisionsWithEvents((int)hallEntity.First().DefaultId!, startDate, endDate))
                return HallRentError.CollisionWithExistingEvent;

            if (await _collisionChecker.CheckTimeCollisionsWithHallRents((int)hallEntity.First().DefaultId!, startDate, endDate, id))
                return HallRentError.CollisionWithExistingHallRent;

            return Error.None;
        }


        // Not used
        protected sealed override Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            throw new NotImplementedException();
        }

        protected override HallRentResponseDto MapResponse(HallRent entity) => entity.ToResponse();

        protected override HallRent MapAsEntity(HallRentRequestDto requestDto) => requestDto.ToEntity();

        protected override HallRent MapUpdatedAsEntity(UpdateHallRentRequestDto requestDto) => requestDto.ToEntity();

        protected override IEntity MapToEntity(UpdateHallRentRequestDto requestDto, HallRent oldEntity) => requestDto.ApplyTo(oldEntity);
    }
}
