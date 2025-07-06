using FinanceManager.Application.Tags.Dtos;
using FluentValidation;

namespace FinanceManager.Application.Tags.Validators;

// ReSharper disable once UnusedType.Global
public class UpdateTagValidator : AbstractValidator<UpdateTagDto>
{
    public UpdateTagValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
    }
}
