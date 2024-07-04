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

namespace FinanceManager.Business.Services;

public class ImportService(FinanceManagerDbContext context, ILogger<ImportService> logger, IMapper mapper)
    : IImportService
{
    public async Task<List<CsvImportResult>> ProcessImport(IEnumerable<IFormFile> files)
    {
        List<CsvImportResult> results = [];
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
        var readerConfig = new CsvConfiguration(new CultureInfo("nl-NL"))
        {
            Delimiter = ","
        };

        using var reader = new StreamReader(file.OpenReadStream());
        using var csv = new CsvReader(reader, readerConfig);

        try
        {
            var records = csv.GetRecords<CsvImportRabo>().ToList();
            if (records.Count > 0)
            {
                result = await ProcessRecords(records);
            }
        }
        catch (BadDataException exception)
        {
            logger.LogError(exception, "Failed to process imported file");
            result = new CsvImportResult();
        }

        return result;
    }

    private async Task<CsvImportResult> ProcessRecords(List<CsvImportRabo> records)
    {
        CsvImportResult result = new();
        foreach (var record in records)
        {
            try
            {
                var account = await GetFromAccount(record);
                var counterpartyAccount = await GetCounterPartyAccount(record);

                var transaction = mapper.Map<Transaction>(record);
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

                await context.Transactions.AddAsync(transaction);
                await context.SaveChangesAsync();

                result.Imported += 1;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed to process record");
                result.Failed.Add(record);
            }
        }

        return result;
    }

    private async Task<Account> GetCounterPartyAccount(CsvImportRabo record)
    {
        var counterpartyAccount = await context.Accounts
            .FirstOrDefaultAsync(acc => (!string.IsNullOrEmpty(acc.Iban) && acc.Iban == record.CounterpartyIban) ||
                                        acc.Name.Equals(record.CounterpartyName,
                                            StringComparison.CurrentCultureIgnoreCase));
        if (counterpartyAccount != null) return counterpartyAccount;

        counterpartyAccount = new Account()
        {
            Iban = record.CounterpartyIban,
            Name = record.CounterpartyName
        };
        await context.Accounts.AddAsync(counterpartyAccount);

        return counterpartyAccount;
    }

    private async Task<Account> GetFromAccount(CsvImportRabo record)
    {
        var account = await context.Accounts
            .FirstOrDefaultAsync(acc =>
                acc.Iban.Equals(record.Iban, StringComparison.CurrentCultureIgnoreCase));
        if (account != null) return account;
            
        account = new Account()
        {
            Iban = record.Iban,
            Name = string.Empty
        };
        await context.Accounts.AddAsync(account);

        return account;
    }
}