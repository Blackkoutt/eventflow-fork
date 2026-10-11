using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.Logic.DTO.Statistics.RequestDto;
using EventFlowAPI.Logic.Helpers;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController(
        IStatisticsService statisticsService, 
        IFileService fileService) : BaseController
    {
        private readonly IStatisticsService _statisticsService = statisticsService;
        private readonly IFileService _fileService = fileService;

        [HttpGet]
        [Authorize(Roles = nameof(Roles.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetStatistics([FromQuery] StatisticsRequestDto requestDto)
        {
            var result = await _statisticsService.GenerateStatistics(requestDto);
            return Ok(result);
        }

        [HttpGet("pdf-report")]
        [Authorize(Roles = nameof(Roles.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetStatisticsPdf([FromQuery] StatisticsRequestDto requestDto)
        {
            var result = await _fileService.CreateStatisticsPDF(requestDto);
            return File(result.Bitmap, ContentType.PDF, result.FileName);
        }
    }
}
