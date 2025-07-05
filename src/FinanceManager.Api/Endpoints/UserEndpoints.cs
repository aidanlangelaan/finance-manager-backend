using FinanceManager.Application.Common.Interfaces;

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

    private static IResult GetMeAsync(ICurrentUserService currentUser, CancellationToken ct)
    {
        if (!currentUser.IsAuthenticated || currentUser.KeycloakId is null)
            return Results.Unauthorized();

        return Results.Ok(new
        {
            UserId = currentUser.KeycloakId,
            Name = currentUser.DisplayName,
            currentUser.Email
        });
    }
}
