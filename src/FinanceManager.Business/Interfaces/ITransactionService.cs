using System.Linq.Expressions;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data.Entities;

namespace FinanceManager.Business.Interfaces;

public interface ITransactionService
{
    public Task<List<GetTransactionDTO>> GetAll();

    public Task<GetTransactionDTO?> GetById(int id);

    public Task<GetTransactionDTO> Create(CreateTransactionDTO model);

    public Task Update(UpdateTransactionDTO model);

    public Task Delete(int id);
    
    public Task<List<GetTransactionDTO>> AssignCategoryToTransaction(AssignCategoryToTransactionDTO model);

    public Expression<Func<Transaction, bool>> FuzzyTransactionLookupClause(Transaction transaction);
}