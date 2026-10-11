using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.Logic.Enums;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(
        IEventService eventService,
        IFileService fileService,
        IHallService hallService) : ControllerBase
    {
        private readonly IEventService _eventService = eventService;
        private readonly IHallService _hallService = hallService;
        private readonly IFileService _fileService = fileService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEvents([FromQuery] EventQuery query)
        {
            var result = await _eventService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.Error.Details);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventById([FromRoute] int id)
        {
            var result = await _eventService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : BadRequest(result.Error.Details);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventReqestDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "startDate": "2024-11-10T13:00:00",
        ///         "endDate": "2024-11-10T15:00:00",
        ///         "name": "string",
        ///         "shortDescription": "string",
        ///         "longDescription": "string",
        ///         "categoryId": 1,
        ///         "hallId": 1,
        ///         "eventTickets": [
        ///           {
        ///             "price": 999.99,
        ///             "ticketTypeId": 0
        ///           }
        ///         ]
        ///     }    
        /// </remarks>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateEvent([FromForm] EventRequestDto eventReqestDto)
        {
            var result = await _eventService.AddAsync(eventReqestDto);
            if (!result.IsSuccessful)
            {
                return result.Error.Details!.Code switch
                {
                    HttpStatusCode.BadRequest => BadRequest(result.Error.Details),
                    HttpStatusCode.Unauthorized => Unauthorized(result.Error.Details),
                    HttpStatusCode.Forbidden => StatusCode((int)HttpStatusCode.Forbidden, result.Error.Details),
                    _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Error.Details)
                };
            }
            return CreatedAtAction(nameof(GetEventById), new { id = result.Value.Id }, result.Value);
        }

        [Authorize]
        [HttpGet("{id:int}/pdf-hallview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPdfHallViewByHallRentId([FromRoute] int id)
        {
            var result = await _fileService.GetFile<Event>(id, FileType.PDF, BlobContainer.HallViewsPDF);
            if (!result.IsSuccessful)
            {
                return result.Error.Details!.Code switch
                {
                    HttpStatusCode.BadRequest => BadRequest(result.Error.Details),
                    HttpStatusCode.Unauthorized => Unauthorized(result.Error.Details),
                    HttpStatusCode.Forbidden => StatusCode((int)HttpStatusCode.Forbidden, result.Error.Details),
                    _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Error.Details)
                };
            }
            return File(result.Value.Data, result.Value.ContentType, result.Value.FileName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eventReqestDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "startDate": "2024-11-10T12:00:00",
        ///         "endDate": "2024-11-10T14:00:00",
        ///         "name": "string",
        ///         "shortDescription": "string",
        ///         "longDescription": "string",
        ///         "categoryId": 1,
        ///         "hallId": 1,
        ///         "eventTickets": [
        ///           {
        ///             "price": 19.99,
        ///             "ticketTypeId": 1
        ///           }
        ///         ]
        ///     }    
        /// </remarks>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEvent([FromRoute] int id, [FromForm] UpdateEventRequestDto eventReqestDto)
        {
            var result = await _eventService.UpdateAsync(id, eventReqestDto);
            if (!result.IsSuccessful)
            {
                return result.Error.Details!.Code switch
                {
                    HttpStatusCode.BadRequest => BadRequest(result.Error.Details),
                    HttpStatusCode.Unauthorized => Unauthorized(result.Error.Details),
                    HttpStatusCode.Forbidden => StatusCode((int)HttpStatusCode.Forbidden, result.Error.Details),
                    _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Error.Details)
                };
            }
            return NoContent();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hallReqestDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "hallDetails": {
        ///           "stageLength": 6, 
        ///           "stageWidth": 6,
        ///           "numberOfSeatsRows": 15,
        ///           "maxNumberOfSeatsRows": 15,
        ///           "numberOfSeatsColumns": 10,
        ///           "maxNumberOfSeatsColumns": 10
        ///         },
        ///         "hallTypeId": 1,
        ///         "seats": [
        ///           {
        ///             "seatNr": 1,
        ///             "row": 1,
        ///             "gridRow": 1,
        ///             "column": 1,
        ///             "gridColumn": 1,
        ///             "seatTypeId": 1
        ///           },
        ///           {
        ///             "seatNr": 2,
        ///             "row": 1,
        ///             "gridRow": 1,
        ///             "column": 2,
        ///             "gridColumn": 2,
        ///             "seatTypeId": 1
        ///           }
        ///         ]
        ///     }    
        /// </remarks>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("{id:int}/hall")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEventHall([FromRoute] int id, [FromBody] EventHallRequestDto hallReqestDto)
        {
            var result = await _hallService.UpdateHallForEvent(id, hallReqestDto);
            if (!result.IsSuccessful)
            {
                return result.Error.Details!.Code switch
                {
                    HttpStatusCode.BadRequest => BadRequest(result.Error.Details),
                    HttpStatusCode.Unauthorized => Unauthorized(result.Error.Details),
                    HttpStatusCode.Forbidden => StatusCode((int)HttpStatusCode.Forbidden, result.Error.Details),
                    _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Error.Details)
                };
            }
            return NoContent();
        }

        [HttpGet("{id:int}/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventImage([FromRoute] int id)
        {
            var result = await _fileService.ValidateAndGetEntityPhoto<Event>(id);
            if (!result.IsSuccessful)
            {
                return result.Error.Details!.Code switch
                {
                    HttpStatusCode.BadRequest => BadRequest(result.Error.Details),
                    _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Error.Details)
                };
            }
            return File(result.Value.Data, result.Value.ContentType, result.Value.FileName);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            var result = await _eventService.DeleteAsync(id);
            if (!result.IsSuccessful)
            {
                return result.Error.Details!.Code switch
                {
                    HttpStatusCode.BadRequest => BadRequest(result.Error.Details),
                    HttpStatusCode.Unauthorized => Unauthorized(result.Error.Details),
                    HttpStatusCode.Forbidden => StatusCode((int)HttpStatusCode.Forbidden, result.Error.Details),
                    _ => StatusCode((int)HttpStatusCode.InternalServerError, result.Error.Details)
                };
            }
            return NoContent();
        }
    }
}
