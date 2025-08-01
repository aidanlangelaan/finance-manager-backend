namespace FinanceManager.Api.ViewModels.ImportJob;

public class CreateImportJobViewModel
{
    public required string OriginalFileName { get; set; }

    public required string MappingProfileJson { get; set; }

    public bool NotifyOnCompletion { get; set; }

    public required IFormFile File { get; set; }
}
