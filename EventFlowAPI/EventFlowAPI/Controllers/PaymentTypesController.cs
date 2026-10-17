using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.DB.Entities.Enums;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypesController(IPaymentTypeService paymentTypeService) : BaseController
    {
        private readonly IPaymentTypeService _paymentTypeService = paymentTypeService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaymentTypes([FromQuery] PaymentTypeQuery query)
        {
            var result = await _paymentTypeService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaymentTypeById([FromRoute] int id)
        {
            var result = await _paymentTypeService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [HttpGet("{id:int}/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventImage([FromRoute] int id)
        {
            var result = await _paymentTypeService.GetImageAsync(id);
            return result.IsSuccessful 
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName)
                : HandleErrorResponse(result);
        }
    }
}