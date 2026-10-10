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
    public class BlobService(LocalStorageSettings storageSettings, IConfiguration configuration) : IBlobService
    {
        private readonly string _staticPath = storageSettings.StaticPath;
        private readonly string _generatedPath = storageSettings.GeneratedPath;
        private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();

        // File name templates come from the "FileNames" section of appsettings.json
        private readonly string _ticketJPGFileNameTemplate = GetFileNameTemplate(configuration, "TicketJPG");
        private readonly string _ticketPDFFileNameTemplate = GetFileNameTemplate(configuration, "TicketPDF");

        private readonly string _eventPassJPGFileNameTemplate = GetFileNameTemplate(configuration, "EventPassJPG");
        private readonly string _eventPassPDFFileNameTemplate = GetFileNameTemplate(configuration, "EventPassPDF");

        public async Task<Result<string>> CreateEventPassBlob(EventPassBlobRequestDto request)
        {
            if (request.ContentType is not (ContentType.PDF or ContentType.JPEG))
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
                    await DeleteAllAddedBlobs(addedBlobs);
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

            var filePath = GetGeneratedFilePath(blob.Container, blob.FileName);
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

            var filePath = ResolveExistingFilePath(blob.Container, blob.FileName);
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
            // Only generated/uploaded files are deleted, static assets are never touched
            var filePath = GetGeneratedFilePath(blob.Container, blob.FileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

        private async Task DeleteAllAddedBlobs(List<BlobRequestDto> blobs)
        {
            foreach (var blob in blobs)
            {
                await DeleteAsync(blob);
            }
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


        // Path.GetFileName strips any directory part, so a file name cannot escape its container folder
        private static string GetFilePath(string rootPath, BlobContainer container, string fileName)
        {
            return Path.Combine(rootPath, container.ToLowerString(), Path.GetFileName(fileName));
        }

        private string GetGeneratedFilePath(BlobContainer container, string fileName)
        {
            return GetFilePath(_generatedPath, container, fileName);
        }

        // Generated (and newly uploaded) files take precedence over the static ones shipped with the repo
        private string ResolveExistingFilePath(BlobContainer container, string fileName)
        {
            var generatedPath = GetGeneratedFilePath(container, fileName);
            return File.Exists(generatedPath) ? generatedPath : GetFilePath(_staticPath, container, fileName);
        }

        private static string GetFileNameTemplate(IConfiguration configuration, string key)
        {
            return configuration.GetSection("FileNames")[key]
                ?? throw new InvalidOperationException($"Missing configuration value 'FileNames:{key}'.");
        }
    }
}
