using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.Data.Entities;

namespace FinanceManager.Business.Interfaces
{
    public interface ITransactionService
    {
        public Task<List<Transaction>> GetAll();

        public Task<Transaction> GetById(int id);

        public Task<Transaction> Create(Transaction model);

        public Task Update(Transaction model);

        public Task Delete(Transaction model);
    }
}
