using FluentValidation;

namespace FinanceManager.Api.Models
{
    public class UpdateTransactionViewModelValidator : AbstractValidator<UpdateTransactionViewModel>
    {
        public UpdateTransactionViewModelValidator()
        {
            RuleFor(transaction => transaction.Id).NotEmpty();
            RuleFor(transaction => transaction.Amount).NotEmpty();
            RuleFor(transaction => transaction.FromAccountId).NotEmpty();
            RuleFor(transaction => transaction.ToAccountId).NotEmpty();
            RuleFor(transaction => transaction.Date).NotEmpty();
        }
    }
}
