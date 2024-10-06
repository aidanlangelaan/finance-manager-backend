namespace FinanceManager.Business.Services.Models;

public class CsvImportResult
{
    public string? FileName { get; set; }

    public int Imported { get; set; }

    public List<CsvImportRabo> Failed { get; } = new();

    public bool IsSuccess => Imported > 0 && Failed.Count == 0;
}