using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using Microsoft.AspNetCore.Authorization;
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
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser(RegisterUserViewModel model)
    {
        await authenticationService.CreateUser(mapper.Map<RegisterUserDTO>(model));

        // always return OK to prevent user enumeration
        return Ok();
    }

    /// <summary>
    /// Confirms the users email address
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/authentication/confirm-email
    ///
    /// </remarks>
    /// <response code="200">The email address for the user has been confirmed</response>
    /// <response code="400">Failed to process request or failed to confirm the users email address</response>
    [HttpPost]
    [Route("confirm-email")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmailAddress(ConfirmEmailAddressViewModel model)
    {
        var result = await authenticationService.ConfirmEmailAddress(mapper.Map<ConfirmEmailAddressDTO>(model));

        if (!result.Succeeded)
        {
            return BadRequest(new { Message = "Invalid or expired confirmation token." });
        }

        return Ok(new { Message = "Email confirmed successfully." });
    }

    /// <summary>
    /// Sets the password for a user
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/authentication/set-password
    ///
    /// </remarks>
    /// <response code="200">The password for a user has been updated</response>
    /// <response code="400">Failed to process request or failed to set users password</response>
    [HttpPost]
    [Route("set-password")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        var result = await authenticationService.ResetPassword(mapper.Map<ResetPasswordDTO>(model));
        if (!result.Succeeded)
        {
            return BadRequest(new { Message = "Failed to reset password. Please check the details and try again." });
        }

        return Ok("Password has been successfully reset.");
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
    ///     POST /api/authentication/refresh-token
    ///
    /// </remarks>
    /// <returns>An authentication token</returns>
    /// <response code="200">User has been verified and an authorization token is returned</response>
    /// <response code="401">User invalid, not found or existing refresh token has expired</response>
    /// <response code="400">Failed to process request</response>
    [HttpPost]
    [Route("refresh-token")]
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