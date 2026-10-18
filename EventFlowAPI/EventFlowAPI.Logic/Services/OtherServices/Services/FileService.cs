using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO.Compression;

namespace EventFlowAPI.Logic.Services.OtherServices.Services
{
    public class FileService(
        IUnitOfWork unitOfWork,
        IBlobService blobService,
        IAuthService authService,
        IPdfBuilderService pdfBuilderService,
        IJPGCreatorService jpgCreatorService,
        IConfiguration configuration
       ) : IFileService
    {
        private readonly IBlobService _blobService = blobService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;   
        private readonly IPdfBuilderService _pdfBuilder = pdfBuilderService;
        private readonly IJPGCreatorService _jpgCreator = jpgCreatorService;

        private string zipArchiveName = @$"twoje_bilety_rezerwacja_nr_{{0}}.zip";
        private string hallRentPDFFileNameTemplate = @$"wynajem_sali_{{0}}.pdf";
        private string hallViewFileNameTemplate = @$"sala_{{0}}.pdf";
        private string _ticketJPGFileNameTemplate => GetFileNameTemplate("TicketJPG");
        private string _ticketPDFFileNameTemplate => GetFileNameTemplate("TicketPDF");
        private string _eventPassJPGFileNameTemplate => GetFileNameTemplate("EventPassJPG");
        private string _eventPassPDFFileNameTemplate => GetFileNameTemplate("EventPassPDF");
        private readonly string defaultPhoto = @"default.jpg";
        

        private Result<BlobContainer> GetPhotoBlobContainer<TEntity>(TEntity entity) where TEntity : class
        {
            return entity switch
            {
                Event => Result<BlobContainer>.Success(BlobContainer.EventPhotos),
                Festival => Result<BlobContainer>.Success(BlobContainer.FestivalPhotos),
                HallType => Result<BlobContainer>.Success(BlobContainer.HallTypePhotos),
                PaymentTypeInfo => Result<BlobContainer>.Success(BlobContainer.PaymentTypePhotos),
                News => Result<BlobContainer>.Success(BlobContainer.NewsPhotos),
                Partner partner => Result<BlobContainer>.Success(partner.Kind switch
                {
                    PartnerKind.MediaPatron => BlobContainer.MediaPatronPhotos,
                    PartnerKind.Organizer => BlobContainer.OrganizerPhotos,
                    PartnerKind.Sponsor => BlobContainer.SponsorPhotos,
                    _ => BlobContainer.PartnerPhotos
                }),
                User => Result<BlobContainer>.Success(BlobContainer.UserPhotos),
                _ => Result<BlobContainer>.Failure(BlobError.NoPhotoContainerForGivenEntityType)
            };
        }

        public async Task<Error> PostPhoto<TEntity>(TEntity entity, IFormFile? file, string fileName, bool isUpdate = false) where TEntity : class
        {
            List<string> availableContentTypes = new List<string>
            {
                ContentType.JPEG,
                ContentType.PNG
            };
            if (file != null && !availableContentTypes.Contains(file.ContentType))
                return Error.BadPhotoFileExtension;

            if (entity is IPhotoEntity photoEntity)
            {
                if (file == null)
                {
                    if(!isUpdate) photoEntity.PhotoName = defaultPhoto;
                    return Error.None;
                }
                if(!fileName.Contains(".jpg") || !fileName.Contains(".png") || !fileName.Contains(".jpeg"))
                {
                    if (file.ContentType.Split('/').Last() == "jpeg") fileName = $"{fileName}.jpg";
                    else fileName = $"{fileName}.{file.ContentType.Split('/').Last()}";
                }

                var blobContainerResult = GetPhotoBlobContainer(entity);
                if (!blobContainerResult.IsSuccessful)
                    return blobContainerResult.Error;
                var blobContainer = blobContainerResult.Value;

                //Log.Information($"Hello1");
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var photoBitmap = memoryStream.ToArray();

                    var photoBlob = new BlobRequestDto
                    {
                        Container = blobContainer,
                        FileName = fileName,
                        ContentType = file.ContentType,
                        Data = photoBitmap
                    };
                    var createBlobResult = await _blobService.UploadAsync(photoBlob);
                    if(!createBlobResult.IsSuccessful)
                        return createBlobResult.Error;
                }

                photoEntity.PhotoName = fileName;
               // Log.Information($"Hello {photoEntity.PhotoName}");
            }
            return Error.None;
        }

        public async Task<Error> DeletePhoto<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity is not IPhotoEntity photoEntity || photoEntity.PhotoName == defaultPhoto) 
                return Error.None;

            var blobContainerResult = GetPhotoBlobContainer(entity);
            if (!blobContainerResult.IsSuccessful)
                return blobContainerResult.Error;

            var container = blobContainerResult.Value;

            var blob = new BlobRequestDto
            {
                Container = container,
                FileName = ((IPhotoEntity)entity).PhotoName,
            };
            await _blobService.DeleteAsync(blob);

            ((IPhotoEntity)entity).PhotoName = defaultPhoto;

            return Error.None;
        }

        public async Task<Result<BlobResponseDto>> GetEntityPhoto<TEntity>(TEntity entity) where TEntity : class
        {
            var blobContainerResult = GetPhotoBlobContainer(entity);
            if (!blobContainerResult.IsSuccessful)
                return Result<BlobResponseDto>.Failure(blobContainerResult.Error);

            var blobContainer = blobContainerResult.Value;

            string fileName = string.Empty;
            if (entity is IPhotoEntity photoEntity)
            {
                fileName = photoEntity.PhotoName;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = defaultPhoto;
            }

            if (string.IsNullOrEmpty(fileName))
                return Result<BlobResponseDto>.Failure(BlobError.FileNameIsEmpty);

            var blobRequest = new BlobRequestDto
            {
                Container = blobContainer,
                FileName = fileName
            };
            var blobDownloadResult = await _blobService.DownloadAsync(blobRequest);
            if (!blobDownloadResult.IsSuccessful)
                return Result<BlobResponseDto>.Failure(blobDownloadResult.Error);

            return Result<BlobResponseDto>.Success(blobDownloadResult.Value);
        }


        public async Task<Result<BlobResponseDto>> ValidateAndGetEntityPhoto<TEntity>(int id) where TEntity : class
        {
            var validationResult = await ValidateBeforeDownload<TEntity>(id, userAllowed: true, authorize: false);
            if (!validationResult.IsSuccessful)
                return Result<BlobResponseDto>.Failure(validationResult.Error);
            var entity = validationResult.Value;

            return await GetEntityPhoto(entity);    
        }

        public async Task<Result<BlobResponseDto>> GetUserPhoto(string id)
        {
            var entity = await _unitOfWork.Users.GetOneAsync(id);
            if(entity == null)
                return Result<BlobResponseDto>.Failure(Error.NotFound);

            return await GetEntityPhoto(entity);
        }


        public async Task<Result<BlobResponseDto>> GetFile<TEntity>(int id, FileType fileType, BlobContainer container) where TEntity : class
        {
            var validationResult = await ValidateBeforeDownload<TEntity>(id);
            if (!validationResult.IsSuccessful)
                return Result<BlobResponseDto>.Failure(validationResult.Error);

            BlobResponseDto? file = validationResult.Value switch
            {
                Hall hall => await CreateHallViewFile(hall, null, null),
                Event eventEntity => await CreateHallViewFile(await GetHall(eventEntity.HallId), null, eventEntity),
                HallRent hallRent when container == BlobContainer.HallRentsPDF => await CreateHallRentFile(hallRent),
                HallRent hallRent => await CreateHallViewFile(await GetHall(hallRent.HallId), hallRent, null),
                Reservation reservation => await CreateTicketPdfFile(reservation),
                EventPass eventPass => await CreateEventPassFile(eventPass, fileType),
                _ => null
            };
            if (file == null)
                return Result<BlobResponseDto>.Failure(BlobError.FileNameIsEmpty);

            return Result<BlobResponseDto>.Success(file);
        }

        public async Task<Result<BlobResponseDto>> GetTicketsJPGsInZIPArchive(int reservationId)
        {
            var validationResult = await ValidateBeforeDownload<Reservation>(reservationId);
            if (!validationResult.IsSuccessful)
                return Result<BlobResponseDto>.Failure(validationResult.Error);

            var reservation = validationResult.Value;
            var tickets = await CreateTicketImages(reservation);

            var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var ticketNumber = 1;
                foreach (var ticketBitmap in tickets.Images)
                {
                    var entry = archive.CreateEntry(string.Format(_ticketJPGFileNameTemplate, reservation.ReservationGuid, ticketNumber), CompressionLevel.Optimal);
                    using (var entryStream = entry.Open())
                    {
                        await entryStream.WriteAsync(ticketBitmap);
                    }
                    ticketNumber++;
                }
            }
            memoryStream.Position = 0;
            var responseObject = new BlobResponseDto
            {
                FileName = string.Format(zipArchiveName, reservation.ReservationGuid),
                Data = memoryStream,
                ContentType = ContentType.ZIP
            };
            return Result<BlobResponseDto>.Success(responseObject);
        }

        private async Task<Hall> GetHall(int id)
        {
            return (await _unitOfWork.GetRepository<Hall>().GetOneAsync(id))!;
        }

        private BlobResponseDto ToFile(byte[] data, string fileName, string contentType)
        {
            return new BlobResponseDto
            {
                FileName = fileName,
                ContentType = contentType,
                Data = new MemoryStream(data)
            };
        }

        private async Task<BlobResponseDto> CreateHallViewFile(Hall hall, HallRent? hallRent, Event? eventEntity)
        {
            var hallViewJPGBitmap = await _jpgCreator.CreateHallJPG(hall);
            var hallViewPDFBitmap = await _pdfBuilder.CreateHallViewPdf(hallViewJPGBitmap, hall, hallRent, eventEntity);

            string key = hall.Id.ToString();
            if (hallRent != null)
                key = $"{hallRent.Id}_{hall.Id}";
            else if (eventEntity != null)
                key = $"{eventEntity.Id}_{hall.Id}";

            return ToFile(hallViewPDFBitmap, string.Format(hallViewFileNameTemplate, key), ContentType.PDF);
        }

        private async Task<BlobResponseDto> CreateHallRentFile(HallRent hallRent)
        {
            var hallRentPDFBitmap = await _pdfBuilder.CreateHallRentPdf(hallRent);

            return ToFile(hallRentPDFBitmap, string.Format(hallRentPDFFileNameTemplate, hallRent.HallRentGuid), ContentType.PDF);
        }

        private async Task<BlobResponseDto> CreateEventPassFile(EventPass eventPass, FileType fileType)
        {
            var eventPassJPGBitmap = await _jpgCreator.CreateEventPass(eventPass);
            if (fileType == FileType.JPEG)
                return ToFile(eventPassJPGBitmap, string.Format(_eventPassJPGFileNameTemplate, eventPass.EventPassGuid), ContentType.JPEG);

            var eventPassPDFBitmap = await _pdfBuilder.CreateEventPassPdf(eventPass, eventPassJPGBitmap, null);
            return ToFile(eventPassPDFBitmap, string.Format(_eventPassPDFFileNameTemplate, eventPass.EventPassGuid), ContentType.PDF);
        }

        private async Task<BlobResponseDto> CreateTicketPdfFile(Reservation reservation)
        {
            var tickets = await CreateTicketImages(reservation);
            var ticketPDFBitmap = await _pdfBuilder.CreateTicketPdf(tickets.Reservation, tickets.Images);

            return ToFile(ticketPDFBitmap, string.Format(_ticketPDFFileNameTemplate, reservation.ReservationGuid), ContentType.PDF);
        }

        private async Task<(Reservation Reservation, List<byte[]> Images)> CreateTicketImages(Reservation reservation)
        {
            var festivalId = reservation.Ticket.FestivalId;
            var festival = festivalId == null ? null : await _unitOfWork.GetRepository<Festival>().GetOneAsync(festivalId.Value);
            if (festival == null)
                return (reservation, [await _jpgCreator.CreateEventTicket(reservation)]);

            var reservationsList = (await _unitOfWork.GetRepository<Reservation>().GetAllAsync(q =>
                                        q.Where(r => r.ReservationGuid == reservation.ReservationGuid && !r.IsDeleted)
                                         .OrderBy(r => r.Id))).ToList();
            return (reservationsList.First(), await _jpgCreator.CreateFestivalTicket(festival, reservationsList));
        }

        private string GetFileNameTemplate(string key)
        {
            return _configuration.GetSection("FileNames")[key]
                ?? throw new InvalidOperationException($"Missing configuration value 'FileNames:{key}'.");
        }

        private async Task<Result<TEntity>> ValidateBeforeDownload<TEntity>(int id, bool userAllowed = false, bool authorize = true) where TEntity : class
        {
            if (id < 0)
                return Result<TEntity>.Failure(Error.RouteParamOutOfRange);

            var entity = await _unitOfWork.GetRepository<TEntity>().GetOneAsync(id);
            if (entity == null)
                return Result<TEntity>.Failure(Error.NotFound);

            /*if (entity is IExpireable expireableEntity && expireableEntity.IsExpired)
                return Result<TEntity>.Failure(Error.EntityIsExpired);*/

            if (entity is ISoftDeleteable deleteableEntity && deleteableEntity.IsDeleted)
                return Result<TEntity>.Failure(Error.EntityIsDeleted);

            string? userId = null;
            if (entity is Hall) userAllowed = true;
            if (entity is IAuthEntity authEntity)
                userId = authEntity.UserId;

            var premissionError = await ValidateUserPremissionToResource(userId, userAllowed, authorize);
            if (premissionError != Error.None)
                return Result<TEntity>.Failure(premissionError);

            return Result<TEntity>.Success(entity);
        }


        private async Task<Error> ValidateUserPremissionToResource(string? userId, bool userAllowed = false, bool authorize = true)
        {
            if (!authorize) return Error.None;
            var userResult = await _authService.GetCurrentUser();
            if (!userResult.IsSuccessful)
                return userResult.Error;

            var user = userResult.Value;
            if (user.IsInRole(Roles.User))
            {
                if (userAllowed) return Error.None;

                if (string.IsNullOrEmpty(userId) || userId != user.Id)
                    return AuthError.UserDoesNotHavePremissionToResource;
            }
            return Error.None;
        }
    }
}
