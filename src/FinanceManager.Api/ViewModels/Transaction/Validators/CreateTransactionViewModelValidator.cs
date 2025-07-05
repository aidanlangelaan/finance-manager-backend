using FluentValidation;

namespace FinanceManager.Api.ViewModels.Transaction.Validators;

public class CreateTransactionViewModelValidator : AbstractValidator<CreateTransactionViewModel>
{
    public CreateTransactionViewModelValidator()
    {
        RuleFor(x => x.SourceAccountId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Source account is required.");

        RuleFor(x => x.DestinationAccountId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Destination account is required.");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Amount is required.");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Description is required and cannot exceed 500 characters.");
    }
}
