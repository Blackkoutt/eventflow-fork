using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.Response;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.Extensions;
using EventFlowAPI.Logic.Enums;

namespace EventFlowAPI.Logic.Services.OtherServices.Services
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _configuration;
        private readonly string _staticPath;
        private readonly string _generatedPath;
        private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

        private readonly string _ticketJPGFileNameTemplate;
        private readonly string _ticketPDFFileNameTemplate;

        private readonly string _eventPassJPGFileNameTemplate;
        private readonly string _eventPassPDFFileNameTemplate;

        public BlobService(LocalStorageSettings storageSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _staticPath = storageSettings.StaticPath;
            _generatedPath = storageSettings.GeneratedPath;

            _ticketJPGFileNameTemplate = GetFileNameTemplate("TicketJPG");
            _ticketPDFFileNameTemplate = GetFileNameTemplate("TicketPDF");
            _eventPassJPGFileNameTemplate = GetFileNameTemplate("EventPassJPG");
            _eventPassPDFFileNameTemplate = GetFileNameTemplate("EventPassPDF");
        }

        public async Task<Result<string>> CreateEventPassBlob(EventPassBlobRequestDto request)
        {
            if (request.ContentType != ContentType.PDF && request.ContentType != ContentType.JPEG)
            {
                return Result<string>.Failure(BlobError.UnsupportedContentType);
            }

            var isPdf = request.ContentType == ContentType.PDF;
            var newBlob = new BlobRequestDto
            {
                Container = isPdf ? BlobContainer.EventPassesPDF : BlobContainer.EventPassesJPG,
                FileName = string.Format(isPdf ? _eventPassPDFFileNameTemplate : _eventPassJPGFileNameTemplate, request.EventPassGuid),
                ContentType = request.ContentType,
                Data = request.Data
            };
            var uploadResult = await UploadAsync(newBlob);
            if (!uploadResult.IsSuccessful)
            {
                return Result<string>.Failure(uploadResult.Error);
            }

            return Result<string>.Success(newBlob.FileName);
        }

        public async Task<Result<List<IFileEntity>>> CreateTicketBlobs(TicketBlobsRequestDto request)
        {
            List<IFileEntity> files = [];
            List<BlobRequestDto> addedBlobs = [];

            var ticketNumber = 1;
            foreach (var data in request.DataList)
            {
                IFileEntity? file = request.ContentType switch
                {
                    ContentType.PDF => new TicketPDF
                    {
                        FileName = string.Format(_ticketPDFFileNameTemplate, request.ReservationGuid),
                        ReservationGuid = request.ReservationGuid
                    },
                    ContentType.JPEG => new TicketJPG
                    {
                        FileName = string.Format(_ticketJPGFileNameTemplate, request.ReservationGuid, ticketNumber),
                        ReservationGuid = request.ReservationGuid
                    },
                    _ => null
                };
                if (file is null)
                {
                    return Result<List<IFileEntity>>.Failure(BlobError.UnsupportedContentType);
                }

                files.Add(file);

                var newBlob = new BlobRequestDto
                {
                    Container = request.Container,
                    FileName = file.FileName,
                    ContentType = request.ContentType,
                    Data = data
                };
                addedBlobs.Add(newBlob);

                var uploadResult = await UploadAsync(newBlob);
                if (!uploadResult.IsSuccessful)
                {
                    foreach (var addedBlob in addedBlobs)
                    {
                        await DeleteAsync(addedBlob);
                    }
                    return Result<List<IFileEntity>>.Failure(uploadResult.Error);
                }

                ticketNumber++;
            }
            return Result<List<IFileEntity>>.Success(files);
        }

        public async Task<Result<object>> UploadAsync(BlobRequestDto blob)
        {
            var blobError = ValidateBlobRequest(blob: blob, isUpload: true);
            if (blobError != Error.None)
            {
                return Result<object>.Failure(blobError);
            }

            var filePath = GetFilePath(blob.Container, blob.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            await File.WriteAllBytesAsync(filePath, blob.Data, blob.CancellationToken);
            return Result<object>.Success();
        }

        public async Task<Result<BlobResponseDto>> DownloadAsync(BlobRequestDto blob)
        {
            var blobError = ValidateBlobRequest(blob: blob, isUpload: false);
            if (blobError != Error.None)
            {
                return Result<BlobResponseDto>.Failure(blobError);
            }

            var filePath = GetFilePath(blob.Container, blob.FileName, useStaticFallback: true);
            if (!File.Exists(filePath))
            {
                var downloadError = new Error(new BadRequestResponse($"Plik '{blob.FileName}' nie istnieje w kontenerze '{blob.Container.ToLowerString()}'."));
                return Result<BlobResponseDto>.Failure(downloadError);
            }

            var data = await File.ReadAllBytesAsync(filePath, blob.CancellationToken);
            if (!_contentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var blobResponse = new BlobResponseDto
            {
                FileName = blob.FileName,
                Data = new MemoryStream(data),
                ContentType = contentType,
            };

            return Result<BlobResponseDto>.Success(blobResponse);
        }

        public Task DeleteAsync(BlobRequestDto blob)
        {
            var filePath = GetFilePath(blob.Container, blob.FileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        private Error ValidateBlobRequest(BlobRequestDto blob, bool isUpload)
        {
            if (string.IsNullOrEmpty(blob.Container.ToLowerString()))
            {
                return BlobError.ContainerIsNullOrEmpty;
            }

            if (string.IsNullOrEmpty(blob.FileName))
            {
                return BlobError.BlobNameIsNullOrEmpty;
            }

            if (isUpload)
            {
                if (blob.Data == null || blob.Data.Length == 0)
                {
                    return BlobError.BlobDataIsNullOrEmpty;
                }

                if (string.IsNullOrEmpty(blob.ContentType))
                {
                    return BlobError.BlobContentTypeIsNullOrEmpty;
                }
            }
            return Error.None;
        }

        private string GetFilePath(BlobContainer container, string fileName, bool useStaticFallback = false)
        {
            var relativePath = Path.Combine(container.ToLowerString(), Path.GetFileName(fileName));
            var generatedFilePath = Path.Combine(_generatedPath, relativePath);
            if (useStaticFallback && !File.Exists(generatedFilePath))
            {
                return Path.Combine(_staticPath, relativePath);
            }

            return generatedFilePath;
        }

        private string GetFileNameTemplate(string key)
        {
            return _configuration.GetSection("FileNames")[key]
                ?? throw new InvalidOperationException($"Missing configuration value 'FileNames:{key}'.");
        }
    }
}
