using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.ResponseDto;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Identity.Helpers;
using EventFlowAPI.Logic.Mapper.Extensions;
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
        IEmailSenderService emailSender,
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
        private readonly IEmailSenderService _emailSender = emailSender;

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
        public async Task<Result<EventPassResponseDto>> BuyEventPass()
        {
            var transactionStatusResult = await CheckBuyTransactionStatus();
            if (!transactionStatusResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(transactionStatusResult.Error);

            var requestDto = transactionStatusResult.Value;

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
                PaymentTypeId = 1,
            };

            // Add EventPass to db
            await AddAsync(eventPass);
            await _unitOfWork.SaveChangesAsync();

            var pdfResult = await CreateJPGAndPDFFileAndUpdateEventPassInDB(eventPass, isUpdate: false);
            if (!pdfResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(pdfResult.Error);

            var pdfBitmap = pdfResult.Value;

            var sendError = await _emailSender.SendInfo(eventPass, EmailType.Create, user.EmailAddress, attachmentData: pdfBitmap);
            if (sendError != Error.None)
                return Result<EventPassResponseDto>.Failure(sendError);
            //await _emailSender.SendEventPassPDFAsync(eventPassEntity, pdfBitmap);

            var eventPassDto = MapAsDto(eventPass);

            return Result<EventPassResponseDto>.Success(eventPassDto);
        }

        private async Task<Result<EventPassRequestDto>> CheckBuyTransactionStatus()
        {
            var passTypeId = _httpContextAccessor.HttpContext!.Session.GetInt32("BuyPassTypeId");
            if (passTypeId == null)
                return Result<EventPassRequestDto>.Failure(EventPassError.SessionError);
            else
                _httpContextAccessor.HttpContext!.Session.Remove("BuyPassTypeId");

            var requestDto = new EventPassRequestDto
            {
                PassTypeId = (int)passTypeId
            };

            return Result<EventPassRequestDto>.Success(requestDto);
        }

        public async Task<Result<PaymentRedirectDto>> CreateBuyEventPassPayment(EventPassRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, null);
            if (validationError != Error.None)
                return Result<PaymentRedirectDto>.Failure(validationError);

            var passType = await _unitOfWork.GetRepository<EventPassType>().GetOneAsync(requestDto!.PassTypeId);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<PaymentRedirectDto>.Failure(userResult.Error);
            var user = userResult.Value;

            var response = new PaymentRedirectDto { RedirectUri = "http://localhost:5173/eventpasses?buy" };
            _httpContextAccessor.HttpContext!.Session.SetInt32("BuyPassTypeId", requestDto.PassTypeId);

            return Result<PaymentRedirectDto>.Success(response);
        }


        public async Task<Result<PaymentRedirectDto>> CreateRenewEventPassPayment(int id, UpdateEventPassRequestDto? requestDto)
        {
            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<PaymentRedirectDto>.Failure(validationError);

            var eventPass = await _repository.GetOneAsync(id);
            if (eventPass == null)
                return Result<PaymentRedirectDto>.Failure(Error.NotFound);

            if (eventPass.IsExpired)
                return Result<PaymentRedirectDto>.Failure(EventPassError.EventPassIsExpired);

            if (eventPass.IsDeleted)
                return Result<PaymentRedirectDto>.Failure(EventPassError.EventPassIsDeleted);

            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return Result<PaymentRedirectDto>.Failure(userResult.Error);

            var user = userResult.Value;
            if (user.IsInRole(Roles.User) && user.Id != eventPass.UserId)
                return Result<PaymentRedirectDto>.Failure(AuthError.UserDoesNotHavePremissionToResource);

            var newPassType = await _unitOfWork.GetRepository<EventPassType>().GetOneAsync(requestDto!.PassTypeId);

            var paymentAmount = Math.Round(newPassType!.Price - (newPassType.Price * (eventPass.PassType.RenewalDiscountPercentage / 100)), 2);
            Log.Information("Price: {Price}, RenewalDiscountPercentage: {RenewalDiscountPercentage}, Calculated Amount: {CalculatedAmount}",
                newPassType.Price,
                eventPass.PassType.RenewalDiscountPercentage,
                paymentAmount);

            var response = new PaymentRedirectDto { RedirectUri = "http://localhost:5173/profile/eventpasses?renew" };
            _httpContextAccessor.HttpContext!.Session.SetInt32("RenewEventPassId", id);
            _httpContextAccessor.HttpContext!.Session.SetInt32("RenewPassTypeId", requestDto.PassTypeId);

            return Result<PaymentRedirectDto>.Success(response);
        }


        private async Task<Result<(int Id, UpdateEventPassRequestDto RequestDto)>> CheckRenewTransactionStatus()
        {
            var id = _httpContextAccessor.HttpContext!.Session.GetInt32("RenewEventPassId");
            var passTypeId = _httpContextAccessor.HttpContext!.Session.GetInt32("RenewPassTypeId");

            if (id == null || passTypeId == null)
            {
                return Result<(int, UpdateEventPassRequestDto)>.Failure(EventPassError.SessionError);
            }
            else
            {
                _httpContextAccessor.HttpContext!.Session.Remove("RenewEventPassId");
                _httpContextAccessor.HttpContext!.Session.Remove("RenewPassTypeId");
            }

            var requestDto = new UpdateEventPassRequestDto
            {
                PassTypeId = (int)passTypeId
            };

            return Result<(int, UpdateEventPassRequestDto)>.Success(((int)id, requestDto));
        }

        // Renew Event Pass
        public async Task<Result<EventPassResponseDto>> RenewEventPass(/*int id, UpdateEventPassRequestDto? requestDto*/)
        {
            Log.Information("Hello from renew");
            var transactionStatusResult = await CheckRenewTransactionStatus();
            if(!transactionStatusResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(transactionStatusResult.Error);

            Log.Information("Hello from renew1");
            var requestDto = transactionStatusResult.Value.RequestDto;
            var id = transactionStatusResult.Value.Id;

            var validationError = await ValidateEntity(requestDto, id);
            if (validationError != Error.None)
                return Result<EventPassResponseDto>.Failure(validationError);

            Log.Information("Hello from renew2");
            var eventPass= await _repository.GetOneAsync((int)id);
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
            var newPaymentType = await _unitOfWork.GetRepository<PaymentType>().GetOneAsync(1);

            var oldEventPassType = new EventPassType
            {
                Name = eventPass.PassType.Name,
                ValidityPeriodInMonths = eventPass.PassType.ValidityPeriodInMonths,
                RenewalDiscountPercentage = eventPass.PassType.RenewalDiscountPercentage,
                Price = eventPass.PassType.Price,
            };

            Log.Information("Hello from renew3");
            UpdateEventPass(eventPass, newPassType!, newPaymentType!);
            Log.Information("Hello from renew4");
            var pdfResult = await CreateJPGAndPDFFileAndUpdateEventPassInDB(eventPass, oldEventPassType, isUpdate: true);
            if (!pdfResult.IsSuccessful)
                return Result<EventPassResponseDto>.Failure(pdfResult.Error);

            var sendError = await _emailSender.SendInfo(eventPass, EmailType.Update, user.EmailAddress, attachmentData: pdfResult.Value);
            if (sendError != Error.None)
                return Result<EventPassResponseDto>.Failure(sendError);
            //await _emailSender.SendEventPassRenewPDFAsync(eventPass, pdfResult.Value);

            var newEventPassDto = MapAsDto(eventPass);
            return Result<EventPassResponseDto>.Success(newEventPassDto);
        }

        private void UpdateEventPass(EventPass eventPass, EventPassType newPassType, PaymentType paymentType)
        {
            var totalDiscountPercentage = eventPass.PassType.RenewalDiscountPercentage;
            var totalDiscount = (newPassType!.Price * (eventPass.PassType.RenewalDiscountPercentage / 100));
            var paymentAmount = Math.Round(newPassType!.Price - (newPassType.Price * (eventPass.PassType.RenewalDiscountPercentage / 100)), 2);

            eventPass.PreviousEndDate = eventPass.EndDate;
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

            var sendError = await _emailSender.SendInfo(eventPass, EmailType.Cancel, eventPass.User.Email!);
            if (sendError != Error.None)
                return Result<object>.Failure(sendError);

            //await _emailSender.SendInfoAboutCanceledEventPass(eventPass);

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

                var ticketJPGRepository = (ITicketJPGRepository)_unitOfWork.GetRepository<TicketJPG>();
                var ticketJPGList = await ticketJPGRepository.GetAllJPGsForReservation(firstRes.Id);
                var deleteTicketJPGsError = await _fileService.DeleteFileEntities(ticketJPGList);
                if (deleteTicketJPGsError != Error.None)
                    return deleteTicketJPGsError;

                var ticketPDFRepository = (ITicketPDFRepository)_unitOfWork.GetRepository<TicketPDF>();
                var ticketPDFList = await ticketPDFRepository.GetAllPDFsForReservation(firstRes.Id);
                var deleteTicketPDFError = await _fileService.DeleteFileEntities(ticketPDFList);
                if (deleteTicketPDFError != Error.None)
                    return deleteTicketPDFError;
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
                var responseDto = entity.AsDto<EventPassResponseDto>();
                responseDto.User = entity.User.AsDto<UserResponseDto>();
                responseDto.User.EmailAddress = entity.User.Email!;
                responseDto.EventPassGuid = entity.EventPassGuid;
                responseDto.EventPassStatus = GetEntityStatus(entity);
                responseDto.User.UserData = null;
                responseDto.PaymentType = entity.PaymentType.AsDto<PaymentTypeResponseDto>();
                responseDto.PassType = entity.PassType.AsDto<EventPassTypeResponseDto>();
                return responseDto;
            });
        }

        protected sealed override EventPassResponseDto MapAsDto(EventPass entity)
        {
            var responseDto = entity.AsDto<EventPassResponseDto>();
            responseDto.User = entity.User.AsDto<UserResponseDto>();
            responseDto.User.EmailAddress = entity.User.Email!;
            responseDto.EventPassStatus = GetEntityStatus(entity);
            responseDto.EventPassGuid = entity.EventPassGuid;
            responseDto.User.UserData = null;
            responseDto.PaymentType = entity.PaymentType.AsDto<PaymentTypeResponseDto>();
            responseDto.PassType = entity.PassType.AsDto<EventPassTypeResponseDto>();
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

            if (!await IsEntityExistInDB<PaymentType>(paymentTypeId))
                return PaymentTypeError.PaymentTypeNotFound;

            if (!await IsEntityExistInDB<EventPassType>(passTypeId))
                return EventPassTypeError.EventPassTypeNotFound;

            return Error.None;
        }
    }
}
