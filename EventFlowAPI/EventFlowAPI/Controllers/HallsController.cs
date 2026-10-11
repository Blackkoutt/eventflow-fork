using EventFlowAPI.Controllers.BaseControllers;
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
    public class HallsController(IHallService hallService, IFileService fileService) : BaseController
    {
        private readonly IHallService _hallService = hallService;
        private readonly IFileService _fileService = fileService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHalls([FromQuery] HallQuery query)
        {
            var result = await _hallService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetHallById([FromRoute] int id)
        {
            var result = await _hallService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [Authorize]
        [HttpGet("{id:int}/pdf-hallview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPdfHallViewByHallId([FromRoute] int id)
        {
            var result = await _fileService.GetFile<Hall>(id, FileType.PDF, BlobContainer.HallViewsPDF);
            return result.IsSuccessful 
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName) 
                : HandleErrorResponse(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hallReqestDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "hallNr": 10,
        ///         "rentalPricePerHour": 599.99,
        ///         "floor": 1,
        ///         "hallDetails": {
        ///            "totalLength": 30,
        ///            "totalWidth": 30,
        ///            "stageLength": 10,
        ///            "stageWidth": 10,
        ///            "numberOfSeatsRows": 10,
        ///            "maxNumberOfSeatsRows": 10,
        ///            "numberOfSeatsColumns": 10,
        ///            "maxNumberOfSeatsColumns": 10
        ///         },
        ///         "hallTypeId": 1,
        ///         "seats": [
        ///             {
        ///                "seatNr": 1,
        ///                "row": 1,
        ///                "gridRow": 1,
        ///                "column": 1,
        ///                "gridColumn": 1,
        ///                "seatTypeId": 1
        ///             },
        ///             {
        ///                "seatNr": 2,
        ///                "row": 2,
        ///                "gridRow": 2,
        ///                "column": 2,
        ///                "gridColumn": 2,
        ///                "seatTypeId": 1
        ///             }
        ///          ]
        ///     }  
        /// </remarks>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateHall([FromBody] HallRequestDto hallReqestDto)
        {
            var result = await _hallService.AddAsync(hallReqestDto);
            return result.IsSuccessful
                ? CreatedAtAction(nameof(GetHallById), new { id = result.Value.Id }, result.Value)
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
        ///         "hallNr": 20,
        ///         "rentalPricePerHour": 599.99,
        ///         "floor": 1,
        ///         "hallDetails": {
        ///            "totalLength": 30,
        ///            "totalWidth": 30,
        ///            "stageLength": 10,
        ///            "stageWidth": 10,
        ///            "numberOfSeatsRows": 10,
        ///            "maxNumberOfSeatsRows": 10,
        ///            "numberOfSeatsColumns": 10,
        ///            "maxNumberOfSeatsColumns": 10
        ///         },
        ///         "hallTypeId": 1,
        ///         "seats": [
        ///             {
        ///                "seatNr": 1,
        ///                "row": 1,
        ///                "gridRow": 1,
        ///                "column": 1,
        ///                "gridColumn": 1,
        ///                "seatTypeId": 1
        ///             }
        ///          ]
        ///     }  
        /// </remarks>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateHall([FromRoute] int id, [FromBody] UpdateHallRequestDto hallReqestDto)
        {
            var result = await _hallService.UpdateAsync(id, hallReqestDto);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }


        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteHall([FromRoute] int id)
        {
            var result = await _hallService.DeleteAsync(id);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }
    }
}