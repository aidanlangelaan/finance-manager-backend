using FluentValidation;

namespace FinanceManager.Api.Models;

public class ResendEmailConfirmationViewModelValidator : AbstractValidator<ResendEmailConfirmationViewModel>
{
    public ResendEmailConfirmationViewModelValidator()
    {
        RuleFor(model => model.EmailAddress).NotEmpty().EmailAddress();
    }
}