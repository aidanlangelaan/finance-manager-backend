namespace FinanceManager.Api.ViewModels.ImportJob;

public class ImportErrorViewModel
{
    public int Id { get; set; }

    public int RowNumber { get; set; }

    public required string RawData { get; set; }

    public required string ErrorMessage { get; set; }
}
