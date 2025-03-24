using FinanceManager.Application.Features.Transactions.GetAllTransactions;
using MediatR;

namespace FinanceManager.Api.Endpoints;

public static class TransactionsEndpoints
{
    public static RouteGroupBuilder MapTransactionEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/transactions", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllTransactionsQuery());
                return Results.Ok(result);
            })
            .WithName("GetAllTransactions")
            .WithOpenApi();

        return group;
    }
}