namespace FinanceManager.Api.ViewModels.ImportJob;

public class ImportJobDetailsViewModel : ImportJobViewModel
{
    public IEnumerable<ImportErrorViewModel> Errors { get; set; } = [];
}
