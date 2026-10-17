using EventFlowAPI.Logic.DTO.Statistics.RequestDto;
using EventFlowAPI.Logic.DTO.Statistics.ResponseDto;
using EventFlowAPI.Logic.Helpers;

namespace EventFlowAPI.Logic.Services.OtherServices.Interfaces
{
    public interface IStatisticsService
    {
        Task<StatisticsResponseDto> GenerateStatistics(StatisticsRequestDto statisticsRequestDto);
    }
}
