using FluentValidation;

namespace FinanceManager.Api.Models
{
    public class CreateTransactionViewModelValidator : AbstractValidator<CreateTransactionViewModel>
    {
        public CreateTransactionViewModelValidator()
        {
            RuleFor(product => product.Amount).NotEmpty();
            RuleFor(product => product.FromAccountId).NotEmpty();
            RuleFor(product => product.ToAccountId).NotEmpty();
            RuleFor(product => product.Date).NotEmpty();
        }
    }
}
