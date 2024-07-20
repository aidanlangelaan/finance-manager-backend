using System.Globalization;
using System.Linq.Expressions;
using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data;
using FinanceManager.Data.Constants;
using FinanceManager.Data.Entities;
using FinanceManager.Data.Enums;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceManager.Business.Services;

public class ImportService(
    FinanceManagerDbContext context,
    ILogger<ImportService> logger,
    IMapper mapper,
    ITransactionService transactionService)
    : IImportService
{
    private static readonly char[] SeparatorChars = [';', '|', '\t', ','];


    public async Task<List<GetImportDTO>> GetAll()
    {
        var imports = await context.Imports.ToListAsync();
        return mapper.Map<List<GetImportDTO>>(imports);
    }

    public async Task<GetImportDTO?> GetById(int id)
    {
        var imports = await context.Imports.FirstOrDefaultAsync(t => t.Id == id);
        return mapper.Map<GetImportDTO>(imports);
    }

    public async Task<bool> SaveImportFile(ImportTransactionsDTO import)
    {
        // validate file
        var fileProvider = new FileExtensionContentTypeProvider();
        if (!fileProvider.TryGetContentType(import.File.FileName, out var contentType)
            || contentType != "text/csv"
            || import.Bank != BankType.Rabobank) // currently only support Rabobank
        {
            return false;
        }

        try
        {
            // save to temp folder with guid name (no extension)!
            var temporaryFileName = $"{Guid.NewGuid()}";
            var filePath = Path.Combine(Path.GetTempPath(), temporaryFileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await import.File.CopyToAsync(stream);

            // add to database
            await context.Imports.AddAsync(new()
            {
                OriginalFileName = import.File.FileName,
                TemporaryFileName = temporaryFileName,
                BankType = import.Bank,
                AssignCategories = import.AssignCategories,
                Status = ImportStatus.Uploaded
            });

            await context.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to save file for processing");
            return false;
        }

        return true;
    }

    public async Task<CsvImportResult?> HandleImports()
    {
        var import = await context.Imports
            .OrderBy(x => x.CreatedOnAt)
            .FirstOrDefaultAsync(x => x.Status == ImportStatus.Uploaded
                                      && x.BankType == BankType.Rabobank);

        if (import == null)
        {
            return null;
        }

        // update status so it can't be picked up by another worker
        import.Status = ImportStatus.Processing;
        await context.SaveChangesAsync();

        // process import
        var result = await ProcessImport(import);

        // update status depending on the result
        import.Status = !result.IsSuccess ? ImportStatus.Failed : ImportStatus.Processed;
        await context.SaveChangesAsync();

        result.FileName = import.OriginalFileName;

        return result;
    }

    private async Task<CsvImportResult> ProcessImport(Import import)
    {
        CsvImportResult result = new();
        try
        {
            // get file
            var filePath = Path.Combine(Path.GetTempPath(), import.TemporaryFileName);
            var lines = File.ReadLines(filePath).ToArray();

            // detect separator and setup reader configuration
            var separator = DetectSeparator(lines);
            var readerConfig = new CsvConfiguration(new CultureInfo("nl-NL"))
            {
                Delimiter = separator.ToString()
            };

            // read file and process records
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, readerConfig);

            var records = csv.GetRecords<CsvImportRabo>().ToList();
            if (records.Count > 0)
            {
                result = await ProcessRecords(records, import);

                if (result.IsSuccess && import.AssignCategories)
                {
                    logger.LogInformation("Assigning categories to imported transactions for import with ID {ImportId}",
                        import.Id);
                    await AssignCategoriesToImportedTransactions(import.Id);
                }
            }
        }
        catch (BadDataException exception)
        {
            logger.LogError(exception, "Failed to process imported file");
        }

        return result;
    }

    private async Task<CsvImportResult> ProcessRecords(List<CsvImportRabo> records, Import import)
    {
        CsvImportResult result = new();
        foreach (var record in records)
        {
            try
            {
                var transaction = mapper.Map<Transaction>(record);
                transaction.ImportId = import.Id;

                var account = await GetFromAccount(record);
                var counterpartyAccount = await GetCounterPartyAccount(record);
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

    private static char DetectSeparator(string[] lines)
    {
        var q = SeparatorChars.Select(sep => new
                { Separator = sep, Found = lines.GroupBy(line => line.Count(ch => ch == sep)) })
            .OrderByDescending(res => res.Found.Count(grp => grp.Key > 0))
            .ThenBy(res => res.Found.Count())
            .FirstOrDefault();

        if (q != null && q.Found.Count() > 1)
        {
            return q.Separator;
        }

        return SeparatorChars[0];
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
                acc.Iban != null && acc.Iban.Equals(record.Iban, StringComparison.CurrentCultureIgnoreCase));
        if (account != null) return account;

        account = new Account()
        {
            Iban = record.Iban,
            Name = string.Empty
        };

        await context.Accounts.AddAsync(account);

        return account;
    }

    private async Task AssignCategoriesToImportedTransactions(int importId)
    {
        var import = await context.Imports
            .Include(import => import.Transactions)
            .FirstOrDefaultAsync(i => i.Id == importId);

        if (import == null)
        {
            return;
        }

        Expression<Func<Transaction, bool>> notUncategorizedPredicate =
            t => t.CategoryId != CategoryConstants.UncategorizedId;
        foreach (var transaction in import.Transactions)
        {
            // build fuzzy predicate
            var fuzzyTransactionLookupClause = transactionService.FuzzyTransactionLookupClause(transaction);
            var fullExpression = Expression.Lambda<Func<Transaction, bool>>(
                Expression.AndAlso(fuzzyTransactionLookupClause, notUncategorizedPredicate),
                fuzzyTransactionLookupClause.Parameters);

            // find matching transactions based on iban and name
            var matchingTransaction = await context.Transactions
                .Where(fullExpression).SingleOrDefaultAsync();

            if (matchingTransaction == null)
            {
                continue;
            }

            await transactionService.AssignCategoryToTransaction(new AssignCategoryToTransactionDTO
                { TransactionId = transaction.Id, CategoryId = transaction.CategoryId, ApplyToSimilarTransactions = true });
        }
    }
}