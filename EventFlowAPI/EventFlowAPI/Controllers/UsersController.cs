using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.Logic.DTO.RequestDto;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using EventFlowAPI.Logic.Query;
using EventFlowAPI.Logic.Services.CRUDServices.Interfaces;
using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.DB.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        IUserService userService,
        IAuthService authService,
        IFileService fileService) : BaseController
    {
        private readonly IAuthService _authService = authService;  
        private readonly IUserService _userService = userService;
        private readonly IFileService _fileService = fileService;

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUsers([FromQuery] UserDataQuery query)
        {
            var result = await _userService.GetAllAsync(query);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var result = await _userService.GetOneAsync(id);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [HttpGet("info")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Info()
        {
            var result = await _authService.GetCurrentUser();
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }

        [HttpGet("info/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserImage()
        {
            var result = await _userService.GetUserPhoto();
            return result.IsSuccessful 
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName) 
                : HandleErrorResponse(result);
        }

        [HttpGet("{id}/image")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserImageById([FromRoute] string id)
        {
            var result = await _fileService.GetUserPhoto(id);
            return result.IsSuccessful
                ? File(result.Value.Data, result.Value.ContentType, result.Value.FileName)
                : HandleErrorResponse(result);
        }

        [HttpPut("info")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> SetUserInfo([FromForm] UserDataRequestDto userDataRequestDto)
        {
            var result = await _userService.SetUserInfo(userDataRequestDto);
            return result.IsSuccessful ? NoContent() : HandleErrorResponse(result);
        }
    }
}