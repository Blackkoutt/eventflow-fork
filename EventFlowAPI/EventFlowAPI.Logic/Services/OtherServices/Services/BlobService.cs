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

        public async Task<Result<List<string>>> CreateTicketBlobs(TicketBlobsRequestDto request)
        {
            List<string> fileNames = [];
            List<BlobRequestDto> addedBlobs = [];

            var ticketNumber = 1;
            foreach (var data in request.DataList)
            {
                string? fileName = request.ContentType switch
                {
                    ContentType.PDF => string.Format(_ticketPDFFileNameTemplate, request.ReservationGuid),
                    ContentType.JPEG => string.Format(_ticketJPGFileNameTemplate, request.ReservationGuid, ticketNumber),
                    _ => null
                };
                if (fileName is null)
                {
                    return Result<List<string>>.Failure(BlobError.UnsupportedContentType);
                }

                fileNames.Add(fileName);

                var newBlob = new BlobRequestDto
                {
                    Container = request.Container,
                    Folder = request.Folder,
                    FileName = fileName,
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
                    return Result<List<string>>.Failure(uploadResult.Error);
                }

                ticketNumber++;
            }
            return Result<List<string>>.Success(fileNames);
        }

        public async Task<Result<object>> UploadAsync(BlobRequestDto blob)
        {
            var blobError = ValidateBlobRequest(blob: blob, isUpload: true);
            if (blobError != Error.None)
            {
                return Result<object>.Failure(blobError);
            }

            var filePath = GetFilePath(blob.Container, blob.FileName, blob.Folder);
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

            var filePath = GetFilePath(blob.Container, blob.FileName, blob.Folder, useStaticFallback: true);
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
            var filePath = GetFilePath(blob.Container, blob.FileName, blob.Folder);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        public Task<List<string>> ListFilesAsync(BlobContainer container, string folder)
        {
            var folderPath = GetFolderPath(container, folder);
            if (!Directory.Exists(folderPath))
            {
                return Task.FromResult(new List<string>());
            }

            var fileNames = Directory.GetFiles(folderPath)
                .Select(Path.GetFileName)
                .OfType<string>()
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToList();
            return Task.FromResult(fileNames);
        }

        public Task DeleteFolderAsync(BlobContainer container, string folder)
        {
            var folderPath = GetFolderPath(container, folder);
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, recursive: true);
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

        private string GetFolderPath(BlobContainer container, string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentException("Folder name cannot be empty.", nameof(folder));
            }
            return Path.Combine(_generatedPath, container.ToLowerString(), Path.GetFileName(folder));
        }

        private string GetFilePath(BlobContainer container, string fileName, string folder = "", bool useStaticFallback = false)
        {
            var relativePath = string.IsNullOrEmpty(folder)
                ? Path.Combine(container.ToLowerString(), Path.GetFileName(fileName))
                : Path.Combine(container.ToLowerString(), Path.GetFileName(folder), Path.GetFileName(fileName));
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
