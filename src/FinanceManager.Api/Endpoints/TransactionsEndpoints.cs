namespace FinanceManager.Api.Endpoints;

public static class TransactionsEndpoints
{
    public static RouteGroupBuilder MapTransactionEndpoints(this RouteGroupBuilder group)
    {
        return group;
    }
    
    public static void MapTransactionEndpointspoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup("/api/transactions")
            .WithTags("Transactions");

        group.MapGet("/", GetAllTransactions);
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