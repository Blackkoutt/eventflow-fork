using EventFlowAPI.DB.Entities.Abstract;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.Logic.ResultObject;
using EventFlowAPI.Logic.Errors;
using Microsoft.AspNetCore.Http;
using EventFlowAPI.Logic.DTO.Statistics.RequestDto;
using EventFlowAPI.Logic.Repositories.Interfaces;
using EventFlowAPI.Logic.Enums;

namespace EventFlowAPI.Logic.Services.OtherServices.Interfaces
{
    public interface IFileService
    {
        Task<Result<BlobResponseDto>> GetTicketsJPGsInZIPArchive(int reservationId);
        Task<Result<BlobResponseDto>> GetFile<TEntity>(int id, FileType fileType, BlobContainer container) where TEntity : class;
        Task<Error> PostPhoto<TEntity>(TEntity entity, IFormFile? file, string fileName, bool isUpdate = false) where TEntity : class;
        Task<Result<BlobResponseDto>> ValidateAndGetEntityPhoto<TEntity>(int id) where TEntity : class;
        Task<Result<BlobResponseDto>> GetEntityPhoto<TEntity>(TEntity entity) where TEntity : class;
        Task<Error> DeletePhoto<TEntity>(TEntity entity) where TEntity : class;
        Task<Result<BlobResponseDto>> GetUserPhoto(string id);
    }
}
