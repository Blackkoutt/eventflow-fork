using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.Logic.Identity.Helpers;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventPassesController(
        IEventPassService eventPassService,
        IFileService fileService) : BaseController
    {
        private readonly IEventPassService _eventPassService = eventPassService;
        private readonly IFileService _fileService = fileService;

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetEventPasses([FromQuery] EventPassQuery query)
        {
            var result = await _eventPassService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetEventPassById([FromRoute] int id)
        {
            var result = await _eventPassService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }
       
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEventPass([FromBody] EventPassRequestDto? eventPassReqestDto)
        {
            var result = await _eventPassService.BuyEventPass(eventPassReqestDto);
            return result.IsSuccessful 
                ? CreatedAtAction(nameof(GetEventPassById), new { id = result.Value.Id }, result.Value) 
                : HandleErrorResponse(result);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CancelEventPass([FromRoute] int id)
        {
            var result = await _eventPassService.DeleteAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RenewEventPass([FromRoute] int id, [FromBody] UpdateEventPassRequestDto? eventPassReqestDto)
        {
            var result = await _eventPassService.RenewEventPass(id, eventPassReqestDto);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpGet("{id:int}/pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetEventPassPdfById([FromRoute] int id)
        {
            var result = await _fileService.GetFile<EventPass>(id, FileType.PDF, BlobContainer.EventPassesPDF);
            return result.IsSuccessful
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName) 
                : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpGet("{id:int}/jpg")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetEventPassJpgById([FromRoute] int id)
        {
            var result = await _fileService.GetFile<EventPass>(id, FileType.JPEG, BlobContainer.EventPassesJPG);
            return result.IsSuccessful
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName)
                : HandleErrorResponse(result);
        }
    }
}
