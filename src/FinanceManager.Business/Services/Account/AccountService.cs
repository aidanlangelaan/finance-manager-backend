using FinanceManager.Business.Interfaces;
using FinanceManager.Data;

namespace FinanceManager.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly FinanceManagerDbContext _context;

        public AccountService(FinanceManagerDbContext context)
        {
            _context = context;
        }
    }
}
