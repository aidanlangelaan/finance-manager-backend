using System;
using FluentValidation;

namespace FinanceManager.Api.Models
{
    public class RegisterUserViewModelValidator : AbstractValidator<RegisterUserViewModel>
    {
        public RegisterUserViewModelValidator()
        {
            RuleFor(register => register.FirstName).NotEmpty();
            RuleFor(register => register.LastName).NotEmpty();
            RuleFor(register => register.EmailAddress).NotEmpty();
            RuleFor(register => register.Password).NotEmpty();
        }
    }
}
