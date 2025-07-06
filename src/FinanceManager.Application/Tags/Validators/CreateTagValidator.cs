using FinanceManager.Application.Tags.Dtos;
using FluentValidation;

namespace FinanceManager.Application.Tags.Validators;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateTagValidator : AbstractValidator<CreateTagDto>
{
    public CreateTagValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
    }
}
