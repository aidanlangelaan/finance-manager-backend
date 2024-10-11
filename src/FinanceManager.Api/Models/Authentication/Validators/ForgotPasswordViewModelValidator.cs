using FluentValidation;

namespace FinanceManager.Api.Models;

public class ForgotPasswordViewModelValidator : AbstractValidator<ForgotPasswordViewModel>
{
    public ForgotPasswordViewModelValidator()
    {
        RuleFor(model => model.EmailAddress).NotEmpty().EmailAddress();
    }
}