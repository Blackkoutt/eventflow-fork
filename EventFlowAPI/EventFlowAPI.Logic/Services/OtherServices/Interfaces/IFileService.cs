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
        Task<(byte[] Bitmap, string FileName)> CreateStatisticsPDF(StatisticsRequestDto statisticsRequestDto);
        Task<Result<(byte[] Bitmap, string FileName)>> CreateEventPassPDFBitmap(EventPass eventPass, byte[] eventPassJPGBitmap, EventPassType? eventPassType, bool isUpdate = false);
        Task<Result<(byte[] Bitmap, string FileName)>> CreateEventPassJPGBitmap(EventPass eventPass, bool isUpdate = false);
        Task<Error> DeleteTicketFiles(Reservation reservation);
        Task<Result<BlobResponseDto>> GetTicketsJPGsInZIPArchive(int reservationId);
        Task<Result<(byte[] Bitmap, string FileName)>> CreateTicketPDFBitmap(Reservation reservationEntity, List<byte[]> ticketBitmaps, bool isUpdate = false);
        Task<Result<(List<byte[]> Bitmaps, List<string> FileNames)>> CreateTicketJPGBitmaps(Festival? festival, List<Reservation> reservationsList, bool isUpdate = false);
        Task<Result<string>> CreateHallViewPDF(Hall hall, HallRent? hallRent = null, Event? eventEntity = null, bool isUpdate = false);
        Task<Result<BlobResponseDto>> GetFile<TEntity>(int id, FileType fileType, BlobContainer container) where TEntity : class;
        Task<Error> DeleteFile<TEntity>(TEntity entity, FileType fileType, BlobContainer container);
        Task<Result<(byte[] PDFFile, string FileName)>> CreateHallRentPDF(HallRent hallRent, bool isUpdate = false);
        Task<Error> PostPhoto<TEntity>(TEntity entity, IFormFile? file, string fileName, bool isUpdate = false) where TEntity : class;
        Task<Result<BlobResponseDto>> ValidateAndGetEntityPhoto<TEntity>(int id) where TEntity : class;
        Task<Result<BlobResponseDto>> GetEntityPhoto<TEntity>(TEntity entity) where TEntity : class;
        Task<Error> DeletePhoto<TEntity>(TEntity entity) where TEntity : class;
        Task<Result<BlobResponseDto>> GetUserPhoto(string id);
    }
}
