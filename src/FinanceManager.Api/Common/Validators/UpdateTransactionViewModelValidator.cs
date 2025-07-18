using FinanceManager.Api.ViewModels.Transaction;
using FluentValidation;

namespace FinanceManager.Api.Common.Validators;

public class UpdateTransactionViewModelValidator : AbstractValidator<UpdateTransactionViewModel>
{
    public UpdateTransactionViewModelValidator()
    {
        RuleFor(x => x.SourceAccountId).GreaterThan(0);
        RuleFor(x => x.DestinationAccountId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.CategoryId).GreaterThan(0).When(x => x.CategoryId.HasValue);
    }
}