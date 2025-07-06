using FinanceManager.Application.Categories.Dtos;
using FluentValidation;

namespace FinanceManager.Application.Categories.Validators;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateCategoryValidator : AbstractValidator<CreateCategoryDto>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
    }
}
