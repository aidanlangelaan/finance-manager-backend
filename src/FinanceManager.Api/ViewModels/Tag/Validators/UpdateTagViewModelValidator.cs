using FluentValidation;

namespace FinanceManager.Api.ViewModels.Tag.Validators;

public class UpdateTagViewModelValidator : AbstractValidator<UpdateTagViewModel>
{
    public UpdateTagViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Account name is required.")
            .MaximumLength(255);
    }
}
