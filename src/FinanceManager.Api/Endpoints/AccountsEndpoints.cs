namespace FinanceManager.Api.Endpoints;

public static class AccountsEndpoints
{
    public static RouteGroupBuilder MapAccountEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/accounts", () =>
            {
                return Results.Ok(new[] {
                    new { Id = 1, Name = "Checking", Balance = 500.00 },
                    new { Id = 2, Name = "Savings", Balance = 1200.00 }
                });
            })
            .WithName("GetAllAccounts")
            .WithOpenApi();

        return group;
    }
}