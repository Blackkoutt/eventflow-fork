using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.Logic.DTO.Statistics.RequestDto;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController(
        IStatisticsService statisticsService) : BaseController
    {
        private readonly IStatisticsService _statisticsService = statisticsService;

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
    }
}
