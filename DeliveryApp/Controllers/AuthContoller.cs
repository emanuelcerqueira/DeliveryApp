using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryApp.Controller.Models;
using DeliveryApp.Data;
using DeliveryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DeliveryApp.Services;
using System.Security.Claims;

namespace DeliveryApp.Controller
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        private readonly IAuthService _authService;

        public AuthController(ITokenService tokenService, IAuthService authService) 
        {
            _tokenService = tokenService;
            _authService = authService;
        }

        /// <summary>
        /// It logins a user
        /// </summary>
        /// <param name="login"></param>
        /// <returns>A JWT token to be used in authenticated/authorized requests</returns>
        /// <response code="200">Returns the newly created token</response>
        /// <response code="401">If wrong credentials</response>
        /// <response code="400">If One or more validation errors occur</response>   
        [Produces("application/json")]
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginRequest login) 
        {
            if (ModelState.IsValid)
            {
                var generatedToken = await _authService.Login(login.Email, login.Password);
                
                if (generatedToken != null)
                    return Ok( new TokenResponse(generatedToken));

                return Unauthorized();
            }

            return BadRequest();
        }

        /// <summary>
        /// Refreshes the JWT token of a logged in user
        /// </summary>
        /// <returns>A JWT token to be used in authenticated/authorized requests</returns>
        /// <response code="200">Returns the newly created token</response>
        /// <response code="401">If user is not logged in</response>
        [Produces("application/json")]
        [HttpPost]
        [Route("refresh-token")]
        [Authorize]
        public ActionResult<TokenResponse> RefreshToken()
        {
            var generatedToken =  _authService.RefreshToken();

            if (generatedToken != null)
                return Ok( new TokenResponse(generatedToken));

            return Unauthorized();
        }


    }
}