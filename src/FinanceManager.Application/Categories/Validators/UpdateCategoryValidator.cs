using FinanceManager.Application.Categories.Dtos;
using FluentValidation;

namespace FinanceManager.Application.Categories.Validators;

// ReSharper disable once UnusedType.Global
public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
    }
}
