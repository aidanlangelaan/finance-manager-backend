using System.Collections.Generic;

namespace FinanceManager.Business.Services.Import;

public class CsvImportResult
{
    public string FileName { get; set; }

    public int Imported { get; set; } = 0;

    public List<CsvImportRabo> Failed { get; set; } = [];

    public bool IsSuccess => Imported > 0 && Failed.Count == 0;
}