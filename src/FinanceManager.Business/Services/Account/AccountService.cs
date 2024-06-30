using FinanceManager.Business.Interfaces;
using FinanceManager.Data;

namespace FinanceManager.Business.Services;

public class AccountService(FinanceManagerDbContext context) : IAccountService
{
}