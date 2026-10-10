using EventFlowAPI.Controllers.BaseControllers;
using EventFlowAPI.Logic.Errors;
using EventFlowAPI.Logic.Identity.DTO.RequestDto;
using EventFlowAPI.Logic.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : BaseController
    {
        private readonly IAuthService _authService = authService;


        [HttpPatch("activate")]
        [Authorize]
        public async Task<IActionResult> ActivateUser()
        {
            var verifyError = await _authService.VerifyUser();
            return verifyError == Error.None ? Ok() : BadRequest(verifyError);
        }


        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateUser()
        {
            var userClaims = User.Claims.ToList();
            return Ok(new
            {
                id = userClaims.FirstOrDefault(c => c.Type == "id")?.Value,
                name = userClaims.FirstOrDefault(c => c.Type == "name")?.Value,
                surname = userClaims.FirstOrDefault(c => c.Type == "surname")?.Value,
                emailAddress = userClaims.FirstOrDefault(c => c.Type == "emailAddress")?.Value,
                dateOfBirth = userClaims.FirstOrDefault(c => c.Type == "dateOfBirth")?.Value,
                isVerified = userClaims.FirstOrDefault(c => c.Type == "isVerified")?.Value,
                userRoles = userClaims.Where(c => c.Type == "userRoles").Select(c => c.Value).ToList()
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRegisterRequestDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "name": "Jan",
        ///         "surname": "Kowalski",
        ///         "email": "eventflow.test@interia.pl",
        ///         "dateOfBirth": "1985-04-25",
        ///         "password": "789!@#qwe",
        ///         "confirmPassword": "789!@#qwe"
        ///     }
        ///       
        /// </remarks>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequestDto userRegisterRequestDto)
        {
            var result = await _authService.RegisterUser(userRegisterRequestDto);
            return result.IsSuccessful ? Ok() : HandleErrorResponse(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginRequestDto"></param>
        /// <returns>JWT token</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "email": "mateusz.strapczuk1@gmail.com",
        ///         "password": "qazzaq1@WSX"
        ///     }
        ///       
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var result = await _authService.Login(loginRequestDto);
            return result.IsSuccessful ? Ok(result.Value) : HandleErrorResponse(result);
        }
    }
}
