using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data;
using FinanceManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Business.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly FinanceManagerDbContext _context;
        private readonly IMapper _mapper;

        public TransactionService(FinanceManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetTransactionDTO>> GetAll()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return _mapper.Map<List<GetTransactionDTO>>(transactions);
        }

        public async Task<GetTransactionDTO> GetById(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            return _mapper.Map<GetTransactionDTO>(transaction);
        }

        public async Task<GetTransactionDTO> Create(CreateTransactionDTO model)
        {
            var transaction = _mapper.Map<Transaction>(model);
            var addTransaction = await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return _mapper.Map<GetTransactionDTO>(addTransaction.Entity);
        }

        public async Task Update(UpdateTransactionDTO model)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == model.Id);
            if (transaction != null)
            {
                transaction = _mapper.Map(model, transaction);
                _context.Transactions.Update(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
