using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.Business.Services.Models;

namespace FinanceManager.Business.Interfaces;

public interface ITransactionService
{
    public Task<List<GetTransactionDTO>> GetAll();

    public Task<GetTransactionDTO> GetById(int id);

    public Task<GetTransactionDTO> Create(CreateTransactionDTO model);

    public Task Update(UpdateTransactionDTO model);

    public Task Delete(int id);
}