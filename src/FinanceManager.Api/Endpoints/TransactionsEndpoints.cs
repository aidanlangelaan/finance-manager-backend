namespace FinanceManager.Api.Endpoints;

public static class TransactionsEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup("/api/transactions")
            .WithTags("Transactions")
            .RequireAuthorization();

        group.MapGet("/", GetAllTransactions)
            .WithName("GetAllTransactions")
            .WithSummary("Retrieve all transactions")
            .WithDescription("This endpoint retrieves a list of all transactions associated with the authenticated user.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> GetAllTransactions(CancellationToken ct)
    {
        // Simulate fetching transactions from a database or service
        var transactions = new[]
        {
            new { Id = 1, AccountId = 1, Amount = 100.00, Description = "Grocery Shopping", Date = DateTime.UtcNow },
            new { Id = 2, AccountId = 2, Amount = -50.00, Description = "Coffee Shop", Date = DateTime.UtcNow }
        };

        return Results.Ok(transactions);
    }
}