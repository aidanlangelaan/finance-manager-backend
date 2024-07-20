using System.Linq.Expressions;
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

    public async Task<GetTransactionDTO?> GetById(int id)
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

    private const int FUZZY_LEVEL = 3;

    public async Task<List<GetTransactionDTO>> AssignCategoryToTransaction(
        AssignCategoryToTransactionDTO model)
    {
        var result = new List<GetTransactionDTO>();

        var transaction = await context.Transactions
            .Include(transaction => transaction.ToAccount)
            .Include(transaction => transaction.FromAccount)
            .FirstOrDefaultAsync(t => t.Id == model.TransactionId);

        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == model.CategoryId);

        if (transaction == null || category == null)
        {
            return result;
        }

        transaction.CategoryId = category.Id;
        context.Transactions.Update(transaction);

        result.Add(mapper.Map<GetTransactionDTO>(transaction));

        if (model.ApplyToSimilarTransactions)
        {
            var transactions = await context.Transactions
                .Include(x => x.FromAccount)
                .Include(x => x.ToAccount)
                .Where(FuzzyTransactionLookupClause(transaction))
                .ToListAsync();

            // TODO: Fuzzy compare using Levenshtein distance, needs improvement before I can use it
            // var similarTransactions = transactions
            //     .Where(t => t.Description != null
            //                 && (t.Description.Normalize().Equals(transaction.Description?.NormalizeValue())
            //                     || t.Description.LevenshteinDistance(transaction.Description?.NormalizeValue()) < FUZZY_LEVEL)
            //     ).ToList();

            if (transactions.Count > 0)
            {
                transactions.ForEach(x => { x.CategoryId = category.Id; });
                context.Transactions.UpdateRange(transactions);

                result.AddRange(mapper.Map<List<GetTransactionDTO>>(transactions));
            }
        }

        await context.SaveChangesAsync();

        return result;
    }

    public Expression<Func<Transaction, bool>> FuzzyTransactionLookupClause(Transaction transaction)
    {
        return t => t.Id != transaction.Id &&
                    // from iban not empty and both from-iban and from-name match
                    (t.FromAccount.Iban != null && t.FromAccount.Iban.Equals(transaction.FromAccount.Iban) &&
                     t.FromAccount.Name.Equals(transaction.FromAccount.Name)
                     // to iban not empty and both to-iban and to-name match
                     && t.ToAccount.Iban != null && t.ToAccount.Iban.Equals(transaction.ToAccount.Iban) &&
                     t.ToAccount.Name.Equals(transaction.ToAccount.Name)
                     // from iban empty but both from-iban and from-name match
                     || (t.FromAccount.Iban == null && t.FromAccount.Name.Equals(transaction.FromAccount.Name))
                     // to iban empty but both to-iban and to-name match
                     || t.ToAccount.Iban == null && t.ToAccount.Name.Equals(transaction.ToAccount.Name));
    }
}