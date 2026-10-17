using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Mapper;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.CRUDServices.Services.BaseServices;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.DTO.Interfaces;
using EventFlowAPI.Logic.Identity.Services.Interfaces;

using Microsoft.AspNetCore.Http;
using Serilog;
using EventFlowAPI.Logic.Enums;

namespace EventFlowAPI.Logic.Services.CRUDServices.Services
{
    public sealed class EventPassService(
        IUnitOfWork unitOfWork,
        IFileService fileService,
        IAuthService authService,
        IHttpContextAccessor httpContextAccessor) :
        GenericService<
            EventPass,
            EventPassRequestDto,
            UpdateEventPassRequestDto,
            EventPassResponseDto,
            EventPassQuery
        >(unitOfWork, authService),
        IEventPassService
    {
        private readonly IFileService _fileService = fileService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public sealed override async Task<Result<IEnumerable<EventPassResponseDto>>> GetAllAsync(EventPassQuery query)
        {
            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<IEnumerable<EventPassResponseDto>>.Failure(userResult.Error);

            var user = userResult.Value;
            if (user.IsInRole(Roles.Admin))
            {
                var allEventPasses = await _repository.GetAllAsync(q => q.ByQuery(query)
                                                                         .GetPage(query.PageNumber, query.PageSize));

                var allEventPassesDto = MapAsDto(allEventPasses);
                return Result<IEnumerable<EventPassResponseDto>>.Success(allEventPassesDto);
            }
            else if (user.IsInRole(Roles.User))
            {
                var userEventPasses = await _repository.GetAllAsync(q =>
                                            q.ByQuery(query)
                                            .Where(r => r.User.Id == user.Id)
                                            .GetPage(query.PageNumber, query.PageSize));

                var userEventPassesResponse = MapAsDto(userEventPasses);
                return Result<IEnumerable<EventPassResponseDto>>.Success(userEventPassesResponse);
            }
            else
            {
                return Result<IEnumerable<EventPassResponseDto>>.Failure(AuthError.UserDoesNotHaveSpecificRole);
            }
        }

        public sealed override async Task<Result<EventPassResponseDto>> GetOneAsync(int id)
        {
            var eventPassResult = await base.GetOneAsync(id);
            if (!eventPassResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(eventPassResult.Error);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(userResult.Error);

            var user = userResult.Value;
            var eventPass = eventPassResult.Value;
            var premissionError = CheckUserPremission(user, eventPass.User!.Id);
            if (premissionError != Error.None)
                return Result<EventPassResponseDto>.Failure(premissionError);

            return Result<EventPassResponseDto>.Success(eventPass);
        }


        // Add Event Pass
        public async Task<Result<EventPassResponseDto>> BuyEventPass(EventPassRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto);
            if (validationError != Error.None)
                return Result<EventPassResponseDto>.Failure(validationError);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(userResult.Error);

            var user = userResult.Value;
            var eventPassType = await _unitOfWork.GetRepository<EventPassType>()
                                        .GetOneAsync(requestDto!.PassTypeId);

            var eventPass = new EventPass
            {
                EventPassGuid = Guid.NewGuid(),
                StartDate = DateTime.Now,
                RenewalDate = null,
                EndDate = DateTime.Now.AddMonths(eventPassType!.ValidityPeriodInMonths),
                PaymentDate = DateTime.Now,
                PaymentAmount = Math.Round(eventPassType!.Price, 2),
                TotalDiscount = 0,
                TotalDiscountPercentage = 0,
                PassTypeId = eventPassType.Id,
                UserId = user.Id,
                PaymentType = PaymentType.PayU,
            };

            // Add EventPass to db
            await AddAsync(eventPass);
            await _unitOfWork.SaveChangesAsync();

            var pdfResult = await CreateJPGAndPDFFileAndUpdateEventPassInDB(eventPass, isUpdate: false);
            if (!pdfResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(pdfResult.Error);

            var eventPassDto = MapAsDto(eventPass);

            return Result<EventPassResponseDto>.Success(eventPassDto);
        }

        // Renew Event Pass
        public async Task<Result<EventPassResponseDto>> RenewEventPass(int id, UpdateEventPassRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<EventPassResponseDto>.Failure(validationError);

            Log.Information("Hello from renew2");
            var eventPass= await _repository.GetOneAsync(id);
            if (eventPass == null)
                return Result<EventPassResponseDto>.Failure(Error.NotFound);

            if(eventPass.IsExpired)
                return Result<EventPassResponseDto>.Failure(EventPassError.EventPassIsExpired);

            if (eventPass.IsDeleted)
                return Result<EventPassResponseDto>.Failure(EventPassError.EventPassIsDeleted);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(userResult.Error);

            var user = userResult.Value;
            if(user.IsInRole(Roles.User) && user.Id != eventPass.UserId)
                return Result<EventPassResponseDto>.Failure(AuthError.UserDoesNotHavePremissionToResource);

            var newPassType = await _unitOfWork.GetRepository<EventPassType>().GetOneAsync(requestDto!.PassTypeId);

            var oldEventPassType = new EventPassType
            {
                Name = eventPass.PassType.Name,
                ValidityPeriodInMonths = eventPass.PassType.ValidityPeriodInMonths,
                RenewalDiscountPercentage = eventPass.PassType.RenewalDiscountPercentage,
                Price = eventPass.PassType.Price,
            };

            Log.Information("Hello from renew3");
            UpdateEventPass(eventPass, newPassType!, PaymentType.PayU);
            Log.Information("Hello from renew4");
            var pdfResult = await CreateJPGAndPDFFileAndUpdateEventPassInDB(eventPass, oldEventPassType, isUpdate: true);
            if (!pdfResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(pdfResult.Error);

            var newEventPassDto = MapAsDto(eventPass);
            return Result<EventPassResponseDto>.Success(newEventPassDto);
        }

        private void UpdateEventPass(EventPass eventPass, EventPassType newPassType, PaymentType paymentType)
        {
            var totalDiscountPercentage = eventPass.PassType.RenewalDiscountPercentage;
            var totalDiscount = (newPassType!.Price * (eventPass.PassType.RenewalDiscountPercentage / 100));
            var paymentAmount = Math.Round(newPassType!.Price - (newPassType.Price * (eventPass.PassType.RenewalDiscountPercentage / 100)), 2);

            eventPass.EndDate = eventPass.EndDate.AddMonths(newPassType!.ValidityPeriodInMonths);
            eventPass.RenewalDate = DateTime.Now;
            eventPass.PaymentDate = DateTime.Now;
            eventPass.PaymentAmount =paymentAmount;
            eventPass.TotalDiscountPercentage = Math.Round(totalDiscountPercentage, 2);
            eventPass.TotalDiscount = Math.Round(totalDiscount, 2);
            eventPass.PassType = newPassType;
            eventPass.PaymentType = paymentType;
        }


        // Admin cancel event pass for user, event pass still will be in db but it will be have a IsDeleted flag
        public sealed override async Task<Result<object>> DeleteAsync(int id)
        {
            var validationResult = await ValidateBeforeDelete(id);
            if (!validationResult.IsSuccessful)
                return Result<object>.Failure(validationResult.Error);

            var eventPass = validationResult.Value.Entity;
            var user = validationResult.Value.User;

            // Remove from PDF AND JPG blob storage
            var jpgDeleteError = await _fileService.DeleteFile(eventPass, FileType.JPEG, BlobContainer.EventPassesJPG);
            if(jpgDeleteError != Error.None)
                return Result<object>.Failure(jpgDeleteError);

            var pdfDeleteError = await _fileService.DeleteFile(eventPass, FileType.PDF, BlobContainer.EventPassesPDF);
            if (pdfDeleteError != Error.None)
                return Result<object>.Failure(pdfDeleteError);

            // Update event pass
            eventPass.EventPassJPGName = string.Empty;
            eventPass.EventPassPDFName = string.Empty;
            eventPass.IsDeleted = true;
            eventPass.DeleteDate = DateTime.Now;
            _repository.Update(eventPass);

            // Cancel all active reservations on event pass
            var reservationDeleteError = await DeleteAllAssociatedActiveReservations(eventPass.Id);
            if (reservationDeleteError != Error.None)
                return Result<object>.Failure(reservationDeleteError);

            await _unitOfWork.SaveChangesAsync();

            return Result<object>.Success();
        }

        private async Task<Error> DeleteAllAssociatedActiveReservations(int eventPassId)
        {
            var reservationRepo = _unitOfWork.GetRepository<Reservation>();
            var allActiveReservations = await reservationRepo
                                                .GetAllAsync(q => q.Where(r =>
                                                r.EventPassId == eventPassId &&
                                                !r.IsDeleted &&
                                                r.EndDate > DateTime.Now));

            if (allActiveReservations.Any())
            {
                var firstRes = allActiveReservations.First();

                foreach (var reservation in allActiveReservations)
                {
                    reservation.IsDeleted = true;
                    reservationRepo.Update(reservation);
                }

                var deleteTicketFilesError = await _fileService.DeleteTicketFiles(firstRes);
                if (deleteTicketFilesError != Error.None)
                    return deleteTicketFilesError;

                firstRes.TicketPDFFileName = null;
                firstRes.TicketImagesFolder = null;
            }
            return Error.None;
        }

        private async Task<EventPassResponseDto> AddAsync(EventPass eventPass)
        {
            await _repository.AddAsync(eventPass);
            var responseDto = MapAsDto(eventPass);
            return responseDto;
        }

        private async Task<Result<byte[]>> CreateJPGAndPDFFileAndUpdateEventPassInDB(EventPass eventPassEntity, EventPassType? oldEventPassType = null, bool isUpdate=false)
        {
            var jpgResult = await _fileService.CreateEventPassJPGBitmap(eventPassEntity, isUpdate);
            if (!jpgResult.IsSuccessful)
                return Result<byte[]>.Failure(jpgResult.Error);

            var jpgBitmap = jpgResult.Value.Bitmap;
            eventPassEntity.EventPassJPGName = jpgResult.Value.FileName;

            var pdfResult = await _fileService.CreateEventPassPDFBitmap(eventPassEntity, jpgBitmap, oldEventPassType, isUpdate);
            if (!pdfResult.IsSuccessful)
                return Result<byte[]>.Failure(pdfResult.Error);

            var pdfBitmap = pdfResult.Value.Bitmap;
            eventPassEntity.EventPassPDFName = pdfResult.Value.FileName;

            _repository.Update(eventPassEntity);
            await _unitOfWork.SaveChangesAsync();

            return Result<byte[]>.Success(pdfBitmap);
        }


        protected sealed override IEnumerable<EventPassResponseDto> MapAsDto(IEnumerable<EventPass> records)
        {
            return records.Select(entity =>
            {
                var responseDto = entity.ToResponse();
                responseDto.User = entity.User.ToResponse();
                responseDto.User.EmailAddress = entity.User.Email!;
                responseDto.EventPassGuid = entity.EventPassGuid;
                responseDto.EventPassStatus = GetEntityStatus(entity);
                responseDto.User.WithoutAddress();
                responseDto.PaymentType = entity.PaymentType.ToResponse();
                responseDto.PassType = entity.PassType.ToResponse();
                return responseDto;
            });
        }

        protected sealed override EventPassResponseDto MapAsDto(EventPass entity)
        {
            var responseDto = entity.ToResponse();
            responseDto.User = entity.User.ToResponse();
            responseDto.User.EmailAddress = entity.User.Email!;
            responseDto.EventPassStatus = GetEntityStatus(entity);
            responseDto.EventPassGuid = entity.EventPassGuid;
            responseDto.User.WithoutAddress();
            responseDto.PaymentType = entity.PaymentType.ToResponse();
            responseDto.PassType = entity.PassType.ToResponse();
            return responseDto;
        }

        protected sealed override async Task<Result<bool>> IsSameEntityExistInDatabase(IRequestDto requestDto, int? id = null)
        {
            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<bool>.Failure(userResult.Error);

            var user = userResult.Value;
            var isUserHaveAnyEventPass = (await _repository.GetAllAsync(q =>
                                            q.Where(ep =>
                                            ep.EndDate > DateTime.Now &&
                                            !ep.IsDeleted &&
                                            ep.UserId == user.Id))).Any();

            return Result<bool>.Success(isUserHaveAnyEventPass);
        }

        protected sealed override async Task<Error> ValidateEntity(IRequestDto? requestDto, int? id = null)
        {
            if (id != null && id < 0)
                return Error.RouteParamOutOfRange;

            if (requestDto == null)
                return Error.NullParameter;

            int paymentTypeId;
            int passTypeId;
            switch (requestDto)
            {
                case EventPassRequestDto eventPassRequestDto:
                    paymentTypeId = 1;
                    passTypeId  = eventPassRequestDto.PassTypeId;
                    break;
                case UpdateEventPassRequestDto updateEventPassRequestDto:
                    paymentTypeId = 1;
                    passTypeId = updateEventPassRequestDto.PassTypeId;
                    break;
                default:
                    return Error.BadRequestType;
            }

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return userResult.Error;

            var activeEventPassResult = await IsSameEntityExistInDatabase(requestDto);
            if (!activeEventPassResult.IsSuccessful) return activeEventPassResult.Error;
            var isUserAlreadyHaveActiveEventPass = activeEventPassResult.Value;

            if (id == null && isUserAlreadyHaveActiveEventPass)
                return EventPassError.UserAlreadyHaveActiveEventPass;

            if (!Enum.IsDefined(typeof(PaymentType), paymentTypeId))
                return PaymentTypeError.PaymentTypeNotFound;

            if (!await IsEntityExistInDB<EventPassType>(passTypeId))
                return EventPassTypeError.EventPassTypeNotFound;

            return Error.None;
        }

        protected override EventPassResponseDto MapResponse(EventPass entity) => entity.ToResponse();

        protected override EventPass MapAsEntity(EventPassRequestDto requestDto) => requestDto.ToEntity();

        protected override EventPass MapUpdatedAsEntity(UpdateEventPassRequestDto requestDto) => requestDto.ToEntity();

        protected override IEntity MapToEntity(UpdateEventPassRequestDto requestDto, EventPass oldEntity) => requestDto.ApplyTo(oldEntity);
    }
}
