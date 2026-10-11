using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
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
    public class FestivalsController(IFestivalService festivalService, IFileService fileService) : BaseController
    {
        private readonly IFestivalService _festivalService = festivalService;
        private readonly IFileService _fileService = fileService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFestivals([FromQuery] FestivalQuery query)
        {
            var result = await _festivalService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFestivalById([FromRoute] int id)
        {
            var result = await _festivalService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="festivalReqestDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "name": "Festiwal nowy",
        ///         "shortDescription": "string",
        ///         "events": 
        ///           {
        ///             "1": null,
        ///             "2": null
        ///           },
        ///         "mediaPatronIds": [
        ///             1, 2
        ///         ],
        ///         "organizerIds": [
        ///             1, 2
        ///         ],
        ///         "sponsorIds": [
        ///             1, 2
        ///         ],
        ///         "details": 
        ///           {
        ///             "longDescription": "string"
        ///           },
        ///         "festivalTickets": [
        ///             {
        ///               "price": 99.99,
        ///               "ticketTypeId": 1
        ///             }
        ///         ]
        ///     }
        /// </remarks>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateFestival([FromForm] FestivalRequestDto festivalReqestDto)
        {
            var result = await _festivalService.AddAsync(festivalReqestDto);
            return result.IsSuccessful 
                ? CreatedAtAction(nameof(GetFestivalById), new { id = result.Value.Id }, result.Value) 
                : HandleErrorResponse(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="festivalReqestDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "name": "Festiwal nowszy",
        ///         "shortDescription": "string",
        ///         "events": 
        ///           {
        ///             "1": 
        ///             {
        ///               "startDate": "2024-11-30T12:00:00Z",
        ///               "endDate": "2024-11-30T14:00:00Z"
        ///             },
        ///             "2": null
        ///           },
        ///         "mediaPatronIds": [
        ///             1, 2
        ///         ],
        ///         "organizerIds": [
        ///             1, 2
        ///         ],
        ///         "sponsorIds": [
        ///             1, 2
        ///         ],
        ///         "details": 
        ///           {
        ///             "longDescription": "string"
        ///           },
        ///         "festivalTickets": [
        ///             {
        ///               "price": 99.99,
        ///               "ticketTypeId": 1
        ///             }
        ///         ]
        ///     }
        /// </remarks>
        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateFestival([FromRoute] int id, [FromForm] UpdateFestivalRequestDto festivalReqestDto)
        {
            var result = await _festivalService.UpdateAsync(id, festivalReqestDto);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteFestival([FromRoute] int id)
        {
            var result = await _festivalService.DeleteAsync(id);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }

        [HttpGet("{id:int}/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFestivalImage([FromRoute] int id)
        {
            var result = await _fileService.ValidateAndGetEntityPhoto<Festival>(id);
            return result.IsSuccessful 
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName) 
                : HandleErrorResponse(result);
        }
    }
}
