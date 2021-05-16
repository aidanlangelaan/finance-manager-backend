using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FinanceManager.Business.Interfaces
{
    public interface IImportService
    {
        public Task<bool> ProcessImport(ICollection<IFormFile> files);
    }
}
