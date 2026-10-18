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

        public BlobService(LocalStorageSettings storageSettings, IConfiguration configuration)
        {
            _configuration = configuration;
            _staticPath = storageSettings.StaticPath;
            _generatedPath = storageSettings.GeneratedPath;
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

    }
}
