using Microsoft.AspNetCore.StaticFiles;
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
    public class BlobService(LocalStorageSettings storageSettings) : IBlobService
    {
        private readonly string _staticPath = storageSettings.StaticPath;
        private readonly string _generatedPath = storageSettings.GeneratedPath;
        private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();
        private string ticketJPGFileNameTemplate = @$"eventflow_bilet_{{0}}_{{1}}.jpg";
        private string ticketPDFFileNameTemplate = @$"eventflow_bilet_{{0}}.pdf";

        private string eventPassJPGFileNameTemplate = @$"eventflow_karnet_{{0}}.jpg";
        private string eventPassPDFFileNameTemplate = @$"eventflow_karnet_{{0}}.pdf";

        public async Task<Result<object>> CreateBlob(string fileName, BlobContainer blobContainer, string contentType, byte[] data, bool isUpdate = false)
        {
            var newBlob = new BlobRequestDto
            {
                Container = blobContainer,
                FileName = fileName,
                ContentType = contentType,
                Data = data
            };
            var uploadResult = await UploadAsync(newBlob, isUpdate: isUpdate);
            if (!uploadResult.IsSuccessful)
                return Result<object>.Failure(uploadResult.Error);

            return Result<object>.Success(fileName);
        }

        public async Task<Result<string>> CreateEventPassBlob(Guid eventPassGuid, byte[] data, string contentType, bool isUpdate=false)
        {
            BlobContainer container;
            string fileName = string.Empty; 
            switch (contentType)
            {
                case ContentType.PDF:
                    container = BlobContainer.EventPassesPDF;
                    fileName = string.Format(eventPassPDFFileNameTemplate, eventPassGuid);
                    break;
                case ContentType.JPEG:
                    container = BlobContainer.EventPassesJPG;
                    fileName = string.Format(eventPassJPGFileNameTemplate, eventPassGuid);
                    break;
                default:
                    return Result<string>.Failure(BlobError.UnsupportedContentType);
            }
            var newBlob = new BlobRequestDto
            {
                Container = container,
                FileName = fileName,
                ContentType = contentType,
                Data = data
            };
            var uploadResult = await UploadAsync(newBlob, isUpdate: isUpdate);
            if (!uploadResult.IsSuccessful)
            {
                return Result<string>.Failure(uploadResult.Error);
            }

            return Result<string>.Success(fileName);
        }


        public async Task<Result<List<IFileEntity>>> CreateTicketBlobs(Guid reservationGuid,
            List<byte[]> dataList, BlobContainer container, string contentType, bool isUpdate = false)
        {
            List<IFileEntity> files = [];
            List<BlobRequestDto> addedBlobs = [];
            IFileEntity file;

            for (var i = 0; i < dataList.Count; i++)
            {
                switch (contentType)
                {
                    case ContentType.PDF:
                        {
                            file = new TicketPDF
                            {
                                FileName = string.Format(ticketPDFFileNameTemplate, reservationGuid),
                                ReservationGuid = reservationGuid
                            };
                            break;
                        }
                    case ContentType.JPEG:
                        {
                            file = new TicketJPG
                            {
                                FileName = string.Format(ticketJPGFileNameTemplate, reservationGuid, i + 1),
                                ReservationGuid = reservationGuid
                            };
                            break;
                        }
                    default:
                        {
                            return Result<List<IFileEntity>>.Failure(BlobError.UnsupportedContentType);
                        }
                }

                files.Add(file);

                var newBlob = new BlobRequestDto
                {
                    Container = container,
                    FileName = file.FileName,
                    ContentType = contentType,
                    Data = dataList[i]
                };
                addedBlobs.Add(newBlob);

                var uploadResult = await UploadAsync(newBlob, isUpdate: isUpdate);
                if (!uploadResult.IsSuccessful)
                {
                    await DeleteAllAddedBlobs(addedBlobs);
                    return Result<List<IFileEntity>>.Failure(uploadResult.Error);
                }
            }
            return Result<List<IFileEntity>>.Success(files);
        }



        public async Task<Result<object>> UploadAsync(BlobRequestDto blob, bool isUpdate = false)
        {
            var blobError = ValidateBlobRequest(blob: blob, isUpload: true);
            if (blobError != Error.None)
                return Result<object>.Failure(blobError);

            var filePath = GetGeneratedFilePath(blob.Container, blob.FileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            await File.WriteAllBytesAsync(filePath, blob.Data, blob.CancellationToken);
            return Result<object>.Success();
        }


        public async Task<Result<BlobResponseDto>> DownloadAsync(BlobRequestDto blob)
        {
            var blobError = ValidateBlobRequest(blob: blob, isUpload: false);
            if (blobError != Error.None)
                return Result<BlobResponseDto>.Failure(blobError);

            var filePath = ResolveExistingFilePath(blob.Container, blob.FileName);
            if (!File.Exists(filePath))
            {
                var downloadError = new Error(new BadRequestResponse($"Plik '{blob.FileName}' nie istnieje w kontenerze '{blob.Container.ToLowerString()}'."));
                return Result<BlobResponseDto>.Failure(downloadError);
            }

            var data = await File.ReadAllBytesAsync(filePath, blob.CancellationToken);
            if (!ContentTypeProvider.TryGetContentType(filePath, out var contentType))
                contentType = "application/octet-stream";

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
                File.Delete(filePath);
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
                return BlobError.ContainerIsNullOrEmpty;

            if (string.IsNullOrEmpty(blob.FileName))
                return BlobError.BlobNameIsNullOrEmpty;

            if (isUpload)
            {
                if (blob.Data == null || blob.Data.Length == 0)
                    return BlobError.BlobDataIsNullOrEmpty;

                if (string.IsNullOrEmpty(blob.ContentType))
                    return BlobError.BlobContentTypeIsNullOrEmpty;
            }
            return Error.None;
        }


        // Path.GetFileName strips any directory part, so a file name cannot escape its container folder
        private static string GetFilePath(string rootPath, BlobContainer container, string fileName)
        {
            return Path.Combine(rootPath, container.ToLowerString(), Path.GetFileName(fileName));
        }

        private string GetGeneratedFilePath(BlobContainer container, string fileName) =>
            GetFilePath(_generatedPath, container, fileName);

        // Generated (and newly uploaded) files take precedence over the static ones shipped with the repo
        private string ResolveExistingFilePath(BlobContainer container, string fileName)
        {
            var generatedPath = GetGeneratedFilePath(container, fileName);
            return File.Exists(generatedPath) ? generatedPath : GetFilePath(_staticPath, container, fileName);
        }
    }
}
