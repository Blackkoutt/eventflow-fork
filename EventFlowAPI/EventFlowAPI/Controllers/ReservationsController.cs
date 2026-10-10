using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.Enums;
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
    public class ReservationsController(
        IReservationService reservationService,
        IFileService fileService) : BaseController
    {
        private readonly IReservationService _reservationService = reservationService;
        private readonly IFileService _fileService = fileService;

        //[Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetReservations([FromQuery] ReservationQuery query)
        {
            var result = await _reservationService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetReservationById([FromRoute] int id)
        {
            var result = await _reservationService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpGet("{id:int}/jpg-tickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTicketsZIPByReservationId([FromRoute] int id)
        {
            var result = await _fileService.GetTicketsJPGsInZIPArchive(id);
            return result.IsSuccessful 
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName)
                : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpGet("{id:int}/pdf-ticket")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTicketPdfByReservationId([FromRoute] int id)
        {
            var result = await _fileService.GetFile<Reservation>(id, FileType.PDF, BlobContainer.TicketsPDF);
            return result.IsSuccessful
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName)
                : HandleErrorResponse(result);    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reservationReqestDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "paymentTypeId": 2,
        ///         "ticketId": 1,
        ///         "isReservationForFestival": false,
        ///         "seatsIds": [
        ///             6,7
        ///          ]
        ///     }
        /// </remarks>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationRequestDto? reservationReqestDto)
        {
            var result = await _reservationService.MakeReservation(reservationReqestDto);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteReservation([FromRoute] int id)
        {
            var result = await _reservationService.DeleteAsync(id);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }
    }
}