namespace FinanceManager.Business.Services.Import;

public class CsvImportResult
{
    public string FileName { get; set; }

    public int Imported { get; set; }

    public List<CsvImportRabo> Failed { get; } = new();

    public bool IsSuccess => Imported > 0 && Failed.Count == 0;
}