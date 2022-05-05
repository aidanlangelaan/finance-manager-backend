using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Login the user to retrieve their JWT token
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/authentication/login
        ///
        /// </remarks>
        /// <returns>An authentication token</returns>
        /// <response code="200">User has been verified and an authorization token is returned</response>
        /// <response code="401">User invalid or not found</response>
        /// <response code="400">Failed to process request</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthorizationTokenViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginUser(LoginUserViewModel model)
        {
            var token = await _authenticationService.LoginUser(_mapper.Map<LoginUserDTO>(model));
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new AuthorizationTokenViewModel(token));
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/authentication/register
        ///
        /// </remarks>
        /// <response code="200">User has been created</response>
        /// <response code="400">Failed to process request or failed to register user</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(AuthorizationTokenViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
        {
            var result = await _authenticationService.RegisterUser(_mapper.Map<RegisterUserDTO>(model));
            if (!result)
            {
                return BadRequest("User already exists or failed to register");
            }
            return Ok();
        }
    }
}
