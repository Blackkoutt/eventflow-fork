using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketTypesController(ITicketTypeService ticketTypeService) : BaseController
    {
        private readonly ITicketTypeService _ticketTypeService = ticketTypeService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTicketTypes([FromQuery] TicketTypeQuery query)
        {
            var result = await _ticketTypeService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTicketTypeById([FromRoute] int id)
        {
            var result = await _ticketTypeService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateTicketType([FromBody] TicketTypeRequestDto ticketTypeReqestDto)
        {
            var result = await _ticketTypeService.AddAsync(ticketTypeReqestDto);
            return result.IsSuccessful 
                ? CreatedAtAction(nameof(GetTicketTypeById), new { id = result.Value.Id }, result.Value)
                : HandleErrorResponse(result);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateTicketType([FromRoute] int id, [FromBody] UpdateTicketTypeRequestDto ticketTypeReqestDto)
        {
            var result = await _ticketTypeService.UpdateAsync(id, ticketTypeReqestDto);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteTicketType([FromRoute] int id)
        {
            var result = await _ticketTypeService.DeleteAsync(id);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }
    }
}