using CsvHelper.Configuration.Attributes;

namespace FinanceManager.Business.Services.Models;

public class CsvImportRabo
{
    [Name("IBAN/BBAN")]
    public string? Iban { get; set; }

    [Name("Munt")]
    public string? Currency { get; set; }

    [Name("Volgnr")]
    public long TransactionCount { get; set; }

    [Name("Datum")]
    public DateTime Date { get; set; }

    [Name("Bedrag")]
    public decimal Amount { get; set; }

    [Name("Tegenrekening IBAN/BBAN")]
    public string? CounterpartyIban { get; set; }

    [Name("Naam tegenpartij")]
    public string? CounterpartyName { get; set; }

    [Name("Code")]
    public string? Code { get; set; }

    [Name("Transactiereferentie")]
    public string? TransactionReference { get; set; }

    [Name("Omschrijving-1")]
    public string? Description { get; set; }
}