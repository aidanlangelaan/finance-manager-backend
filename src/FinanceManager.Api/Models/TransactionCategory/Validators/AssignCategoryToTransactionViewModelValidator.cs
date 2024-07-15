using FluentValidation;

namespace FinanceManager.Api.Models;

public class AssignCategoryToTransactionViewModelValidator : AbstractValidator<AssignCategoryToTransactionViewModel>
{
    public AssignCategoryToTransactionViewModelValidator()
    {
        RuleFor(transaction => transaction.TransactionId).NotEmpty().GreaterThan(0);
        RuleFor(transaction => transaction.CategoryId).NotEmpty().GreaterThan(0);
    }
}