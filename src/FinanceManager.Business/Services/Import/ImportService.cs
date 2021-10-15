using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Import;
using FinanceManager.Data;
using FinanceManager.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceManager.Business.Services
{
    public class ImportService : IImportService
    {
        private readonly FinanceManagerDbContext _context;
        private readonly ILogger<ImportService> _logger;
        private readonly IMapper _mapper;

        public ImportService(FinanceManagerDbContext context, ILogger<ImportService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<CsvImportResult>> ProcessImport(ICollection<IFormFile> files)
        {
            var results = new List<CsvImportResult>();
            foreach (var file in files)
            {
                var result = await ProcessFile(file);
                result.FileName = file.FileName;
                results.Add(result);
            }
            return results;
        }

        private async Task<CsvImportResult> ProcessFile(IFormFile file)
        {
            CsvImportResult result = null;
            var readerConfig = new CsvConfiguration(new CultureInfo("nl-NL"));
            readerConfig.Delimiter = ",";

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, readerConfig))
            {
                try
                {
                    var records = csv.GetRecords<CsvImportRabo>().ToList();
                    if (records.Count > 0)
                    {
                        result = await ProcessRecords(records);
                    }
                }
                catch (BadDataException badDataException)
                {
                    _logger.LogError("Failed to process imported file", badDataException.ToString());
                    result = new CsvImportResult();
                }
            }

            return result;
        }

        private async Task<CsvImportResult> ProcessRecords(List<CsvImportRabo> records)
        {
            var result = new CsvImportResult();

            foreach (var record in records)
            {
                try
                {
                    Account account = await GetFromAccount(record);
                    Account counterpartyAccount = await GetCounterPartyAccount(record);

                    var transaction = _mapper.Map<Transaction>(record);
                    if (transaction.Amount < 0)
                    {
                        transaction.ToAccount = counterpartyAccount;
                        transaction.FromAccount = account;
                    }
                    else
                    {
                        transaction.ToAccount = account;
                        transaction.FromAccount = counterpartyAccount;
                    }

                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();

                    result.Imported += 1;
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed to process record", e.ToString());
                    result.Failed.Add(record);
                }
            }

            return result;
        }

        private async Task<Account> GetCounterPartyAccount(CsvImportRabo record)
        {
            var counterpartyAccount = await _context.Accounts
                                    .FirstOrDefaultAsync(a => (!string.IsNullOrEmpty(a.Iban) && a.Iban == record.CounterpartyIban) ||
                                        a.Name.ToLower() == record.CounterpartyName.ToLower());
            if (counterpartyAccount == null)
            {
                counterpartyAccount = new Account()
                {
                    Iban = record.CounterpartyIban,
                    Name = record.CounterpartyName
                };
                _context.Accounts.Add(counterpartyAccount);
            }

            return counterpartyAccount;
        }

        private async Task<Account> GetFromAccount(CsvImportRabo record)
        {
            var account = await _context.Accounts
                                    .FirstOrDefaultAsync(a =>
                                        a.Iban == record.Iban);
            if (account == null)
            {
                account = new Account()
                {
                    Iban = record.Iban,
                    Name = String.Empty
                };

                _context.Accounts.Add(account);
            }

            return account;
        }
    }
}
