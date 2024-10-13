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
    /// Register a new user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/authentication/register
    ///     {
    ///         "firstName": "John",
    ///         "lastName": "Doe",
    ///         "emailAddress": "john.doe@example.com"
    ///     }
    ///
    /// Request Body:
    /// - **firstName**: The first name of the user (required).
    /// - **lastName**: The last name of the user (required).
    /// - **emailAddress**: The user's email address (required).
    /// </remarks>
    /// <response code="201">User has been created successfully, and the account is pending email confirmation.</response>
    /// <response code="400">Failed to process the request due to validation errors or other issues (e.g., duplicate email).</response>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser(RegisterUserViewModel model)
    {
        var result = await authenticationService.CreateUser(mapper.Map<RegisterUserDTO>(model));
        if (result.Succeeded)
        {
            // always return success code to prevent user enumeration
            return StatusCode(StatusCodes.Status201Created);
        }

        return BadRequest(new { Message = "Failed to register user. Please try again later." });
    }

    /// <summary>
    /// Resends the email confirmation token to the user's email address.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/authentication/resend-confirmation
    ///     {
    ///         "emailAddress": "user@example.com"
    ///     }
    ///
    /// Request Body:
    /// - **emailAddress**: The user's email address (required).
    /// </remarks>
    /// <response code="200">The email confirmation token has been resent.</response>
    /// <response code="400">Failed to resend the email confirmation token.</response>
    [HttpPost]
    [Route("resend-confirmation")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationViewModel model)
    {
        var result = await authenticationService.ResendEmailConfirmation(mapper.Map<ResendEmailConfirmationDTO>(model));
        if (result.Succeeded)
        {
            return Ok(new { Message = "Email confirmation token has been resent." });
        }

        return BadRequest(new { Message = "Failed to resend email confirmation token." });
    }
    
    /// <summary>
    /// Confirms the user's email address using a confirmation token.
    /// </summary>
    /// <remarks>
    /// To confirm the user's email, send a POST request with the confirmation token received via email.
    /// 
    /// Sample request:
    ///
    ///     POST /api/authentication/confirm-email
    ///     {
    ///         "token": "eyJhbGciOiJIUzI1NiIsInR5..."
    ///     }
    ///
    /// Request Body:
    /// - **token**: The email confirmation token (required). This token is usually sent to the user's email address after registration.
    /// </remarks>
    /// <response code="200">The email address for the user has been confirmed.</response>
    /// <response code="400">Failed to confirm the user's email address. This may happen if the token is invalid or expired.</response>
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
    /// Sends a password reset token to the user's email address.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/authentication/forgot-password
    ///     {
    ///         "emailAddress": "user@example.com"
    ///     }
    ///
    /// Request Body:
    /// - **emailAddress**: The user's email address (required).
    /// </remarks>
    /// <response code="200">A password reset token has been sent to the user’s email address.</response>
    /// <response code="400">Failed to process password reset request.</response>
    [HttpPost]
    [Route("forgot-password")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        var result = await authenticationService.ForgotPassword(mapper.Map<ForgotPasswordDTO>(model));

        if (result.Succeeded)
        {
            return Ok(new { Message = "Password reset token sent." });
        }

        return BadRequest(new { Message = "Failed to process password reset request." });
    }
    
    /// <summary>
    /// Resets the user's password using a reset token.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/authentication/reset-password
    ///     {
    ///         "token": "reset_token",
    ///         "password": "new_password"
    ///     }
    ///
    /// Request Body:
    /// - **token**: The password reset token (required).
    /// - **password**: The new password to be set (required).
    /// </remarks>
    /// <response code="200">The password has been successfully reset.</response>
    /// <response code="400">Failed to reset password.</response>
    [HttpPost]
    [Route("reset-password")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        var result = await authenticationService.ResetPassword(mapper.Map<ResetPasswordDTO>(model));

        if (result.Succeeded)
        {
            return Ok(new { Message = "Password has been successfully reset." });
        }

        return BadRequest(new { Message = "Failed to reset password." });
    }

    /// <summary>
    /// Authenticates a user and returns a JWT authorization token if the login is successful.
    /// </summary>
    /// <remarks>
    /// To log in, provide the user's email address and password in the request body.
    /// 
    /// Sample request:
    ///
    ///     POST /api/authentication/login
    ///     {
    ///         "emailAddress": "user@example.com",
    ///         "password": "UserPassword123!"
    ///     }
    ///
    /// Request Body:
    /// - **emailAddress**: The email address of the user (required).
    /// - **password**: The password of the user (required).
    ///
    /// Sample response:
    ///
    ///     {
    ///         "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///         "refreshToken": "def50200f3..."
    ///     }
    /// </remarks>
    /// <returns>An authorization token if the login is successful.</returns>
    /// <response code="200">User has been verified and an authorization token is returned.</response>
    /// <response code="400">Failed to process the request due to invalid data.</response>
    /// <response code="401">User credentials are invalid or the user is not found.</response>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(AuthorizationTokenViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
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
    /// Refreshes the user's access token using the provided refresh token.
    /// </summary>
    /// <remarks>
    /// To refresh the access token, provide the current access token and refresh token in the request body.
    /// 
    /// Sample request:
    ///
    ///     POST /api/authentication/refresh-token
    ///     {
    ///         "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///         "refreshToken": "def50200f3..."
    ///     }
    ///
    /// Request Body:
    /// - **accessToken**: The user's current access token (required).
    /// - **refreshToken**: The refresh token previously issued to the user (required).
    ///
    /// Sample response:
    ///
    ///     {
    ///         "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///         "refreshToken": "def50200f3..."
    ///     }
    /// </remarks>
    /// <returns>An authorization token (access and refresh) if the refresh token is valid.</returns>
    /// <response code="200">User has been verified, and a new authorization token is returned.</response>
    /// <response code="400">Failed to process the request due to invalid data.</response>
    /// <response code="401">The user is invalid, not found, or the refresh token has expired or is invalid.</response>
    [HttpPost]
    [Route("refresh-token")]
    [ProducesResponseType(typeof(AuthorizationTokenViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
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