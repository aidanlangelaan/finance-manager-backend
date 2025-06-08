namespace FinanceManager.Api.Endpoints;

public static class AccountsEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup("/api/accounts")
            .WithTags("Accounts");

        group.MapGet("/", GetAllAccountsAsync)
            .WithName("GetAllAccounts")
            .WithSummary("Retrieve all accounts")
            .WithDescription("This endpoint retrieves a list of all accounts associated with the authenticated user.")
            .Produces<List<string>>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status400BadRequest);
    }
    
    private static async Task<IResult> GetAllAccountsAsync(CancellationToken ct)
    {
        // Simulate fetching accounts from a database or service
        var accounts = new[]
        {
            new { Id = 1, Name = "Checking", Balance = 500.00 },
            new { Id = 2, Name = "Savings", Balance = 1200.00 }
        };

        return Results.Ok(accounts);
    }
}