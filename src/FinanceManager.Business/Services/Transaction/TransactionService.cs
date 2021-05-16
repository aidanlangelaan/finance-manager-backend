using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.Business.Interfaces;
using FinanceManager.Data;
using FinanceManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly FinanceManagerDbContext _context;

        public TransactionService(FinanceManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAll() =>
            await _context.Transactions.ToListAsync();

        public async Task<Transaction> GetById(int id) =>
            await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);

        public async Task<Transaction> Create(Transaction model)
        {
            var transaction = await _context.Transactions.AddAsync(model);
            await _context.SaveChangesAsync();
            return transaction.Entity;
        }

        public async Task Update(Transaction model)
        {
            _context.Transactions.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Transaction model)
        {
            _context.Transactions.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
