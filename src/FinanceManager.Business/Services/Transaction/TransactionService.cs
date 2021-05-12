using FinanceManager.Business.Interfaces;
using FinanceManager.Data;

namespace FinanceManager.Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly FinanceManagerDbContext _context;

        public TransactionService(FinanceManagerDbContext context)
        {
            _context = context;
        }
    }
}
