using System.Threading.Tasks;
using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController(IAuthenticationService authenticationService, IMapper mapper)
    : ControllerBase
{
    

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
        var result = await authenticationService.RegisterUser(mapper.Map<RegisterUserDTO>(model));
        if (!result)
        {
            return BadRequest("User already exists or failed to register");
        }
        return Ok();
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
        var token = await authenticationService.LoginUser(mapper.Map<LoginUserDTO>(model));
        if (token == null)
        {
            return Unauthorized();
        }
        return Ok(mapper.Map<AuthorizationTokenViewModel>(token));
    }
    
    /// <summary>
    /// Refresh the users access token using the provided refresh token
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/authentication/refresh
    ///
    /// </remarks>
    /// <returns>An authentication token</returns>
    /// <response code="200">User has been verified and an authorization token is returned</response>
    /// <response code="401">User invalid, not found or existing refreshtoken has expired</response>
    /// <response code="400">Failed to process request</response>
    [HttpPost]
    [Route("refresh")]
    [ProducesResponseType(typeof(AuthorizationTokenViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshAccessToken(AuthorizationTokenViewModel model)
    {
        var token = await authenticationService.RefreshAccessToken(mapper.Map<RefreshAccessTokenDTO>(model));
        if (token == null)
        {
            return Unauthorized();
        }
        return Ok(mapper.Map<AuthorizationTokenViewModel>(token));
    }
}