using MediatR;

namespace FinanceManager.Application.Features.Transactions.GetAllTransactions;

public class GetAllTransactionsHandler : IRequestHandler<GetAllTransactionsQuery, List<GetAllTransactionsResponse>>
{
    public Task<List<GetAllTransactionsResponse>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        // Temporary mock data (replace with repository/DbContext later)
        var transactions = new List<GetAllTransactionsResponse>
        {
            new(Guid.NewGuid(), 45.99m, "Groceries"),
            new(Guid.NewGuid(), 1200m, "Salary"),
            new(Guid.NewGuid(), 75m, "Utilities"),
        };

        return Task.FromResult(transactions);
    }
}