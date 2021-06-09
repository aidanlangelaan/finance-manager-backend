using System;

namespace FinanceManager.Api
{
    public class TransactionViewViewModel
    {
        public decimal Amount { get; set; }

        public decimal BalanceAfter { get; set; }

        public int FromAccountId { get; set; }

        public int ToAccountId { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
    }
}
