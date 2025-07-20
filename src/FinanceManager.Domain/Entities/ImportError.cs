namespace FinanceManager.Domain.Entities;

public class ImportError : EntityBase
{
    public int ImportJobId { get; set; }

    public int RowNumber { get; set; }

    public string RawData { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public DateTime CreatedOnAt { get; set; }

    // Foreign keys
    public ImportJob ImportJob { get; set; } = null!;
}
