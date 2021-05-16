using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Import;
using FinanceManager.Data;
using Microsoft.AspNetCore.Http;

namespace FinanceManager.Business.Services
{
    public class ImportService : IImportService
    {
        private readonly FinanceManagerDbContext _context;

        public ImportService(FinanceManagerDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ProcessImport(ICollection<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    await ProcessFile(file);
                }
            }

            return true;
        }

        private async Task ProcessFile(IFormFile file)
        {
            var readerConfig = new CsvConfiguration(new CultureInfo("nl-NL"));
            readerConfig.Delimiter = ",";

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, readerConfig))
            {
                try
                {
                    var records = csv.GetRecords<CsvImportRabo>().ToList();
                }
                catch(BadDataException badDataException)
                {
                    // TODO: do something with the bad data
                }
            }
        }
    }
}
