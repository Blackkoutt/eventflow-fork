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
    public class HallRentsController(
        IHallRentService hallRentService,
        IHallService hallService,
        IFileService fileService
        ) : BaseController
    {
        private IHallRentService _hallRentService = hallRentService;
        private IHallService _hallService = hallService;
        private IFileService _fileService = fileService;

        //[Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetHallRents([FromQuery] HallRentQuery query)
        {
            var result = await _hallRentService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }


        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetHallRentById([FromRoute] int id)
        {
            var result = await _hallRentService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hallRentReqestDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "startDate": "2024-10-31T12:00:00Z",
        ///         "endDate": "2024-10-31T14:00:00Z",
        ///         "paymentTypeId": 2,
        ///         "hallId": 1,
        ///         "additionalServicesIds": [
        ///             1,2
        ///          ]
        ///     }
        /// </remarks>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateHallRent([FromBody] HallRentRequestDto? hallRentReqestDto)
        {
            var result = await _hallRentService.MakeRent(hallRentReqestDto);
            return result.IsSuccessful 
                ? CreatedAtAction(nameof(GetHallRentById), new { id = result.Value.Id }, result.Value) 
                : HandleErrorResponse(result);
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
        ///         "hallDetails": 
        ///         {
        ///             "stageLength": 4,
        ///             "stageWidth": 20,
        ///             "numberOfSeatsRows": 10,
        ///             "maxNumberOfSeatsRows": 15,
        ///             "numberOfSeatsColumns": 10,
        ///             "maxNumberOfSeatsColumns": 15,
        ///         },
        ///         "hallTypeId": 1,
        ///         "seats": [
        ///           {
        ///             "seatNr": 1,
        ///             "row": 3,
        ///             "gridRow": 3,
        ///             "column": 3,
        ///             "gridColumn": 3,
        ///             "seatTypeId": 1
        ///           },
        ///           {
        ///             "seatNr": 2,
        ///             "row": 1,
        ///             "gridRow": 1,
        ///             "column": 2,
        ///             "gridColumn": 2,
        ///             "seatTypeId": 2
        ///           },
        ///           {
        ///             "seatNr": 3,
        ///             "row": 5,
        ///             "gridRow": 5,
        ///             "column": 5,
        ///             "gridColumn": 5,
        ///             "seatTypeId": 3
        ///           }
        ///         ]
        ///     }    
        /// </remarks>
        [Authorize]
        [HttpPut("{id:int}/hall")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateRentHall([FromRoute] int id, [FromBody] HallRent_HallRequestDto hallReqestDto)
        {
            var result = await _hallService.UpdateHallForRent(id, hallReqestDto);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }


        [Authorize]
        [HttpGet("{id:int}/pdf-hallview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPdfHallViewByHallRentId([FromRoute] int id)
        {
            var result = await _fileService.GetFile<HallRent>(id, FileType.PDF, BlobContainer.HallViewsPDF);
            return result.IsSuccessful 
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName) 
                : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpGet("{id:int}/pdf-invoice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPdfInoviceByHallRentId([FromRoute] int id)
        {
            var result = await _fileService.GetFile<HallRent>(id, FileType.PDF, BlobContainer.HallRentsPDF);
            return result.IsSuccessful
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName)
                : HandleErrorResponse(result);
        }


        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteHallRent([FromRoute] int id)
        {
            var result = await _hallRentService.DeleteAsync(id);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }
    }
}