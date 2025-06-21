using System.Security.Claims;

namespace FinanceManager.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup("/api/user")
            .WithTags("User")
            .RequireAuthorization();

        group.MapGet("/me", GetMeAsync)
            .WithName("GetMe")
            .WithSummary("Returns current user info")
            .WithDescription("Returns details about the currently authenticated user.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
    
    private static async Task<IResult> GetMeAsync(HttpContext httpContext, CancellationToken ct)
    {
        var sub = httpContext.User.FindFirst("sub")?.Value;
        var username = httpContext.User.FindFirst("preferred_username")?.Value;
        var email = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(sub))
        {
            return Results.Unauthorized();
        }

        return Results.Ok(new
        {
            Sub = sub,
            PreferredUsername = username,
            Email = email
        });
    }
}