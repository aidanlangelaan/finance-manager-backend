using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.Business.Services.Import;
using Microsoft.AspNetCore.Http;

namespace FinanceManager.Business.Interfaces;

public interface IImportService
{
    public Task<List<CsvImportResult>> ProcessImport(IEnumerable<IFormFile> files);
}