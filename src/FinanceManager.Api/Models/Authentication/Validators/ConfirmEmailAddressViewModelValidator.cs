using FluentValidation;

namespace FinanceManager.Api.Models;

public class ConfirmEmailAddressViewModelValidator : AbstractValidator<ConfirmEmailAddressViewModel>
{
    public ConfirmEmailAddressViewModelValidator()
    {
        RuleFor(model => model.Token).NotEmpty();
    }
}