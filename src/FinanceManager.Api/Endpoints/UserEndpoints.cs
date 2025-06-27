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
    
    private static IResult GetMeAsync(HttpContext httpContext, CancellationToken ct)
    {
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var name = httpContext.User.FindFirst("name")?.Value;
        var email = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }

        return Results.Ok(new
        {
            UserId = userId,
            Name = name,
            Email = email
        });
    }
}