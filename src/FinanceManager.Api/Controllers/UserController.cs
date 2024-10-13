using System.Security.Claims;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IAuthenticationService authenticationService)
    : ControllerBase
{
    /// <summary>
    /// Logs out the authenticated user by invalidating their refresh tokens.
    /// </summary>
    /// <remarks>
    /// This endpoint logs out the currently authenticated user by invalidating any active refresh tokens associated with their account.
    /// 
    /// This action requires the user to be authenticated. The user is identified via the authentication token provided in the request header.
    /// 
    /// Sample request:
    ///
    ///     POST /api/user/logout
    ///
    /// There is no request body for this endpoint as the user's ID is extracted from the token provided in the request header.
    /// </remarks>
    /// <response code="200">The user has been logged out successfully, and all refresh tokens have been invalidated.</response>
    /// <response code="400">The logout operation failed due to a server error or invalid user state.</response>
    [HttpPost]
    [Route("logout")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LogoutUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await authenticationService.LogoutUser(new LogoutUserDTO { UserId = userId });

        if (result.Succeeded)
        {
            return Ok(new { Message = "User logged out successfully." });
        }

        return BadRequest(new { Message = "Failed to log out user." });
    }
}