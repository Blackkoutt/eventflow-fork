using EventFlowAPI.DB.Entities;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.Statistics.RequestDto;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Identity.Helpers;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;
using Microsoft.AspNetCore.Http;
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
        IStatisticsService statisticsService
       ) : IFileService
    {
        private readonly IBlobService _blobService = blobService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthService _authService = authService;   
        private readonly IPdfBuilderService _pdfBuilder = pdfBuilderService;
        private readonly IJPGCreatorService _jpgCreator = jpgCreatorService;
        private readonly IStatisticsService _statisticsService = statisticsService;

        private string zipArchiveName = @$"twoje_bilety_rezerwacja_nr_{{0}}.zip";
        private string hallRentPDFFileNameTemplate = @$"wynajem_sali_{{0}}.pdf";
        private readonly string defaultPhoto = @"default.jpg";
        
        public async Task<(byte[] Bitmap, string FileName)> CreateStatisticsPDF(StatisticsRequestDto statisticsRequestDto)
        {
            var statisticsPDFData = await _statisticsService.GenerateDataForStatisticsPDF(statisticsRequestDto);

            var statisticsPDFBitmap = await _pdfBuilder.CreateStatisticsPdf(statisticsPDFData);

            var fileName = $"raport_{statisticsPDFData.StatisticsResponseDto.ReportGuid}.pdf";

            return (statisticsPDFBitmap, fileName); 
        }


        // To Do
        private async Task<string?> GetPDFFileName<TEntity>(TEntity entity, FileType fileType)
        {
            return entity switch
            {
                HallRent hallRent => fileType switch 
                {
                    FileType.PDF => hallRent.HallRentPDFName,
                    _ => throw new ArgumentOutOfRangeException(nameof(fileType), $"Unsupported file type: {fileType}")
                },
                Hall hall => fileType switch
                {
                    FileType.PDF => hall.HallViewFileName,
                    _ => throw new ArgumentOutOfRangeException(nameof(fileType), $"Unsupported file type: {fileType}")
                },
                Reservation reservation => fileType switch
                {
                    FileType.PDF => (await _unitOfWork.GetRepository<TicketPDF>().GetAllAsync(q => q.Where(t =>
                                     t.Reservations.Any(r => r.Id == reservation.Id)))).FirstOrDefault()?.FileName,
                    _ => throw new ArgumentOutOfRangeException(nameof(fileType), $"Unsupported file type: {fileType}")
                },
                EventPass eventPass => fileType switch
                {
                    FileType.PDF => eventPass.EventPassPDFName,
                    FileType.JPEG => eventPass.EventPassJPGName,
                    _ => throw new ArgumentOutOfRangeException(nameof(fileType), $"Unsupported file type: {fileType}")
                },
                _ => throw new ArgumentOutOfRangeException(nameof(entity), $"Unsupported type of entity: {entity!.GetType().Name}")
            };
        }


        public async Task<Result<BlobResponseDto>> GetFile<TEntity>(int id, FileType fileType, BlobContainer container) where TEntity : class
        {
            var validationResult = await ValidateBeforeDownload<TEntity>(id);
            if (!validationResult.IsSuccessful)
                return Result<BlobResponseDto>.Failure(validationResult.Error);

            var entity = validationResult.Value;

            string? fileName = string.Empty;
            if(container == BlobContainer.HallViewsPDF && entity is HallRent hallRent) 
                fileName = await GetPDFFileName(hallRent.Hall, fileType);
            else if(container == BlobContainer.HallViewsPDF && entity is Event eventEntity)
                fileName = await GetPDFFileName(eventEntity.Hall, fileType);
            else 
                fileName = await GetPDFFileName(entity, fileType);
                
            if (string.IsNullOrEmpty(fileName))
                return Result<BlobResponseDto>.Failure(BlobError.FileNameIsEmpty);

            var blobRequest = new BlobRequestDto
            {
                Container = container,
                FileName = fileName
            };
            var ticketPDFResult = await _blobService.DownloadAsync(blobRequest);
            if (!ticketPDFResult.IsSuccessful)
                return Result<BlobResponseDto>.Failure(ticketPDFResult.Error);

            return Result<BlobResponseDto>.Success(ticketPDFResult.Value);
        }


        public async Task<Error> DeleteFile<TEntity>(TEntity entity, FileType fileType, BlobContainer container)
        {
            string? fileName = string.Empty;
            if (container == BlobContainer.HallViewsPDF && entity is HallRent hallRent)
                fileName = await GetPDFFileName(hallRent.Hall, fileType);
            else if (container == BlobContainer.HallViewsPDF && entity is Event eventEntity)
                fileName = await GetPDFFileName(eventEntity.Hall, fileType);
            else
                fileName = await GetPDFFileName(entity, fileType);

            if (string.IsNullOrEmpty(fileName))
                return BlobError.FileNameIsEmpty;

            var blob = new BlobRequestDto
            {
                Container = container,
                FileName = fileName,
            };
            await _blobService.DeleteAsync(blob);

            return Error.None;
        }

        private Result<BlobContainer> GetPhotoBlobContainer<TEntity>(TEntity entity) where TEntity : class
        {
            return entity switch
            {
                Event => Result<BlobContainer>.Success(BlobContainer.EventPhotos),
                Festival => Result<BlobContainer>.Success(BlobContainer.FestivalPhotos),
                HallType => Result<BlobContainer>.Success(BlobContainer.HallTypePhotos),
                MediaPatron => Result<BlobContainer>.Success(BlobContainer.MediaPatronPhotos),
                Organizer => Result<BlobContainer>.Success(BlobContainer.OrganizerPhotos),
                PaymentType => Result<BlobContainer>.Success(BlobContainer.PaymentTypePhotos),
                Sponsor => Result<BlobContainer>.Success(BlobContainer.SponsorPhotos),
                News => Result<BlobContainer>.Success(BlobContainer.NewsPhotos),
                Partner => Result<BlobContainer>.Success(BlobContainer.PartnerPhotos),
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
            var entity = await ((IUserRepository)_unitOfWork.GetRepository<User>()).GetOneAsync(id);
            if(entity == null)
                return Result<BlobResponseDto>.Failure(Error.NotFound);

            return await GetEntityPhoto(entity);
        }


        public async Task<Result<(byte[] PDFFile, string FileName)>> CreateHallRentPDF(HallRent hallRent, bool isUpdate = false)
        {
            var hallRentPDFBitmap = await _pdfBuilder.CreateHallRentPdf(hallRent);

            string fileName = string.Format(hallRentPDFFileNameTemplate, hallRent.HallRentGuid);

            var hallRentPdfBlob = new BlobRequestDto
            {
                Container = BlobContainer.HallRentsPDF,
                FileName = fileName,
                ContentType = ContentType.PDF,
                Data = hallRentPDFBitmap
            };
            var pdfBlobResult = await _blobService.UploadAsync(hallRentPdfBlob);

            if (!pdfBlobResult.IsSuccessful)
                return Result<(byte[], string)>.Failure(pdfBlobResult.Error);

            return Result<(byte[], string)>.Success((hallRentPDFBitmap, fileName));
        }

        public async Task<Result<string>> CreateHallViewPDF(Hall hall, HallRent? hallRent = null, Event? eventEntity = null, bool isUpdate = false)
        {
            var hallViewJPGBitmap = await _jpgCreator.CreateHallJPG(hall);

            var hallViewPDFBitmap = await _pdfBuilder.CreateHallViewPdf(hallViewJPGBitmap, hall, hallRent, eventEntity);

            string fileName = string.Empty;
            string fileNameTemplate = @$"sala_{{0}}.pdf";

            if (hallRent != null)
                fileName = string.Format(fileNameTemplate, $"{hallRent.Id}_{hall.Id}");
            else if (eventEntity != null)
                fileName = string.Format(fileNameTemplate, $"{eventEntity.Id}_{hall.Id}");
            else
                fileName = string.Format(fileNameTemplate, hall.Id);

            hall.HallViewFileName = fileName;

            var hallViewPdfBlob = new BlobRequestDto
            {
                Container = BlobContainer.HallViewsPDF,
                FileName = fileName,
                ContentType = ContentType.PDF,
                Data = hallViewPDFBitmap
            };
            var pdfBlobResult = await _blobService.UploadAsync(hallViewPdfBlob);

            if (!pdfBlobResult.IsSuccessful)
                return Result<string>.Failure(pdfBlobResult.Error);

            return Result<string>.Success(fileName);
        }


        public async Task<Result<BlobResponseDto>> GetTicketsJPGsInZIPArchive(int reservationId)
        {
            var validationResult = await ValidateBeforeDownload<Reservation>(reservationId);
            if (!validationResult.IsSuccessful)
                return Result<BlobResponseDto>.Failure(validationResult.Error);

            var ticketJPGs = await _unitOfWork.GetRepository<TicketJPG>()
                                 .GetAllAsync(q => q.Where(t => t.Reservations.Any(r => r.Id == reservationId)));

            if (ticketJPGs == null || !ticketJPGs.Any())
            {
                return Result<BlobResponseDto>.Failure(TicketJPGError.TicketJPGNotFound);
            }

            var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var ticketJPG in ticketJPGs)
                {
                    var blobRequest = new BlobRequestDto
                    {
                        Container = BlobContainer.TicketsJPG,
                        FileName = ticketJPG.FileName
                    };

                    var ticketJPGResult = await _blobService.DownloadAsync(blobRequest);
                    if (!ticketJPGResult.IsSuccessful)
                    {
                        return Result<BlobResponseDto>.Failure(ticketJPGResult.Error);
                    }

                    var ticketJPGBitmap = ticketJPGResult.Value.Data;
                    var entry = archive.CreateEntry(ticketJPG.FileName, CompressionLevel.Optimal);

                    using (var entryStream = entry.Open())
                    {
                        await ticketJPGBitmap.CopyToAsync(entryStream);
                    }
                }
            }
            memoryStream.Position = 0;
            var responseObject = new BlobResponseDto
            {
                FileName = string.Format(zipArchiveName, ticketJPGs.First().ReservationGuid),
                Data = memoryStream,
                ContentType = ContentType.ZIP

            };
            return Result<BlobResponseDto>.Success(responseObject);
        }


        public async Task<Error> DeleteFileEntities(IEnumerable<IFileEntity> fileEntities)
        {
            BlobContainer containerName;
            ITicketJPGRepository? _ticketJPGRepository = null;
            ITicketPDFRepository? _ticketPDFRepository = null;

            switch (fileEntities)
            {
                case IEnumerable<TicketJPG>:
                    containerName = BlobContainer.TicketsJPG;
                    _ticketJPGRepository = (ITicketJPGRepository)_unitOfWork.GetRepository<TicketJPG>();
                    break;

                case IEnumerable<TicketPDF>:
                    containerName = BlobContainer.TicketsPDF;
                    _ticketPDFRepository = (ITicketPDFRepository)_unitOfWork.GetRepository<TicketPDF>();
                    break;

                default:
                    return BlobError.UnsupportedContainerName;
            }
            foreach (var ticket in fileEntities)
            {
                var blob = new BlobRequestDto
                {
                    Container = containerName,
                    FileName = ticket.FileName,
                };
                await _blobService.DeleteAsync(blob);
                _ticketJPGRepository?.Delete((TicketJPG)ticket);
                _ticketPDFRepository?.Delete((TicketPDF)ticket);
            }

            return Error.None;
        }


        public async Task<Result<(byte[] Bitmap, string FileName)>> CreateEventPassJPGBitmap(EventPass eventPass, bool isUpdate=false)
        {
            var eventPassBitmap = await _jpgCreator.CreateEventPass(eventPass);

            var eventPassJpgBlob = new EventPassBlobRequestDto
            {
                EventPassGuid = eventPass.EventPassGuid,
                ContentType = ContentType.JPEG,
                Data = eventPassBitmap
            };
            var eventPassBlobResult = await _blobService.CreateEventPassBlob(eventPassJpgBlob);
            if (!eventPassBlobResult.IsSuccessful)
            {
                return Result<(byte[] Bitmap, string FileName)>.Failure(eventPassBlobResult.Error);
            }
            var fileName = eventPassBlobResult.Value;

            return Result<(byte[] Bitmap, string FileName)>.Success((eventPassBitmap, fileName));
        }

        public async Task<Result<(byte[] Bitmap, string FileName)>> CreateEventPassPDFBitmap(EventPass eventPass, byte[] eventPassJPGBitmap, EventPassType? oldEventPassType, bool isUpdate = false)
        {
            var eventPassPDFBitmap = await _pdfBuilder.CreateEventPassPdf(eventPass, eventPassJPGBitmap, oldEventPassType);

            var eventPassPdfBlob = new EventPassBlobRequestDto
            {
                EventPassGuid = eventPass.EventPassGuid,
                ContentType = ContentType.PDF,
                Data = eventPassPDFBitmap
            };
            var eventPassBlobResult = await _blobService.CreateEventPassBlob(eventPassPdfBlob);
            if (!eventPassBlobResult.IsSuccessful)
            {
                return Result<(byte[] Bitmap, string FileName)>.Failure(eventPassBlobResult.Error);
            }
            var fileName = eventPassBlobResult.Value;

            return Result<(byte[] Bitmap, string FileName)>.Success((eventPassPDFBitmap, fileName));
        }


        public async Task<Result<(byte[] Bitmap, IFileEntity FileEntity)>> CreateTicketPDFBitmapAndEntity(Reservation reservationEntity, List<byte[]> ticketBitmaps, bool isUpdate = false)
        {
            var ticketPDFBitmap = await _pdfBuilder.CreateTicketPdf(reservationEntity, ticketBitmaps);

            var ticketPdfBlobs = new TicketBlobsRequestDto
            {
                ReservationGuid = reservationEntity.ReservationGuid,
                Container = BlobContainer.TicketsPDF,
                ContentType = ContentType.PDF,
                DataList = new List<byte[]> { ticketPDFBitmap }
            };
            var pdfBlobResult = await _blobService.CreateTicketBlobs(ticketPdfBlobs);
            if (!pdfBlobResult.IsSuccessful)
            {
                return Result<(byte[] Bitmap, IFileEntity FileEntity)>.Failure(pdfBlobResult.Error);
            }
            var fileEntity = pdfBlobResult.Value.First();

            return Result<(byte[] Bitmap, IFileEntity FileEntity)>.Success((ticketPDFBitmap, fileEntity));
        }


        public async Task<Result<(List<byte[]> Bitmaps, List<IFileEntity> FileEntities)>> CreateTicketJPGBitmapsAndEntities(Festival? festival, List<Reservation> reservationsList, bool isUpdate = false)
        {
            List<byte[]> ticketBitmaps = [];
            Reservation reservationEntity = reservationsList.First();

            if (festival is not null)
            {
                ticketBitmaps = await _jpgCreator.CreateFestivalTicket(festival, reservationsList);
            }
            else
            {
                var ticketBitmap = await _jpgCreator.CreateEventTicket(reservationEntity);
                ticketBitmaps.Add(ticketBitmap);
            }

            var ticketJpgBlobs = new TicketBlobsRequestDto
            {
                ReservationGuid = reservationEntity.ReservationGuid,
                Container = BlobContainer.TicketsJPG,
                ContentType = ContentType.JPEG,
                DataList = ticketBitmaps
            };
            var jpgBlobResult = await _blobService.CreateTicketBlobs(ticketJpgBlobs);

            if (!jpgBlobResult.IsSuccessful)
            {
                return Result<(List<byte[]> Bitmaps, List<IFileEntity> FileEntities)>.Failure(jpgBlobResult.Error);
            }
            var fileEntities = jpgBlobResult.Value;

            return Result<(List<byte[]> Bitmaps, List<IFileEntity> FileEntities)>.Success((ticketBitmaps, fileEntities));
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
