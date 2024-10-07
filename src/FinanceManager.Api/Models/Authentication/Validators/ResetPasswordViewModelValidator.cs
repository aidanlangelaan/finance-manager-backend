using FluentValidation;

namespace FinanceManager.Api.Models;

public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
{
    public ResetPasswordViewModelValidator()
    {
        RuleFor(model => model.Password).NotEmpty();
        RuleFor(model => model.Token).NotEmpty();
    }
}