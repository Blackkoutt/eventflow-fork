using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.DTO.UpdateRequestDto;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController(IEquipmentService equipmentService) : BaseController
    {
        private readonly IEquipmentService _equipmentService = equipmentService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEquipments([FromQuery] EquipmentQuery query)
        {
            var result = await _equipmentService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);    
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEquipmentById([FromRoute] int id)
        {
            var result = await _equipmentService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }


        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateEquipment([FromBody] EquipmentRequestDto equipmentReqestDto)
        {
            var result = await _equipmentService.AddAsync(equipmentReqestDto);
            return result.IsSuccessful 
                ? CreatedAtAction(nameof(GetEquipmentById), new { id = result.Value.Id }, result.Value) 
                : HandleErrorResponse(result);
        }


        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEquipment([FromRoute] int id, [FromBody] UpdateEquipmentRequestDto equipmentReqestDto)
        {
            var result = await _equipmentService.UpdateAsync(id, equipmentReqestDto);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }


        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteEquipment([FromRoute] int id)
        {
            var result = await _equipmentService.DeleteAsync(id);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }
    }
}
