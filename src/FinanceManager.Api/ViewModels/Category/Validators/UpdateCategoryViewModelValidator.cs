using FluentValidation;

namespace FinanceManager.Api.ViewModels.Category.Validators;

// ReSharper disable once UnusedType.Global
public class UpdateCategoryViewModelValidator : AbstractValidator<UpdateCategoryViewModel>
{
    public UpdateCategoryViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MaximumLength(255);
    }
}
