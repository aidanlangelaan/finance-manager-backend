using FluentValidation;

namespace FinanceManager.Api.ViewModels.Account.Validators;

public class UpdateAccountViewModelValidator : AbstractValidator<UpdateAccountViewModel>
{
    public UpdateAccountViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Account name is required.")
            .MaximumLength(255);

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Account type is invalid.");

        RuleFor(x => x.Description)
            .MaximumLength(255);
    }
}
