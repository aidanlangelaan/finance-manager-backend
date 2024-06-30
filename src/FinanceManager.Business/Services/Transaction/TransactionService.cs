using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data;
using FinanceManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Business.Services;

public class TransactionService(FinanceManagerDbContext context, IMapper mapper) : ITransactionService
{
    public async Task<List<GetTransactionDTO>> GetAll()
    {
        var transactions = await context.Transactions.ToListAsync();
        return mapper.Map<List<GetTransactionDTO>>(transactions);
    }

    public async Task<GetTransactionDTO> GetById(int id)
    {
        var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        return mapper.Map<GetTransactionDTO>(transaction);
    }

    public async Task<GetTransactionDTO> Create(CreateTransactionDTO model)
    {
        var transaction = mapper.Map<Transaction>(model);
        var addTransaction = await context.Transactions.AddAsync(transaction);
        await context.SaveChangesAsync();
        return mapper.Map<GetTransactionDTO>(addTransaction.Entity);
    }

    public async Task Update(UpdateTransactionDTO model)
    {
        var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.Id == model.Id);
        if (transaction != null)
        {
            transaction = mapper.Map(model, transaction);
            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        var transaction = await context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        if (transaction != null)
        {
            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();
        }
    }
}