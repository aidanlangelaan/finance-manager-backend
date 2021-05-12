using FinanceManager.Business.Interfaces;
using FinanceManager.Data;

namespace FinanceManager.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly FinanceManagerDbContext _context;

        public CategoryService(FinanceManagerDbContext context)
        {
            _context = context;
        }
    }
}
