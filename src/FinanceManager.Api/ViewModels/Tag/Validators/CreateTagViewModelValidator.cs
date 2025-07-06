using FluentValidation;

namespace FinanceManager.Api.ViewModels.Tag.Validators;

public class CreateTagViewModelValidator : AbstractValidator<CreateTagViewModel>
{
    public CreateTagViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Account name is required.")
            .MaximumLength(255);
    }
}
