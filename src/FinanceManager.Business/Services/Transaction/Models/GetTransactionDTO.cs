using System;
using FinanceManager.Data.Constants;

namespace FinanceManager.Business.Services.Models
{
    public class GetTransactionDTO
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public int FromAccountId { get; set; }

        public int ToAccountId { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; } = CategoryConstants.UncategorizedId;
    }
}
