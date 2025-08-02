using System.Text.Json;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Api.ViewModels.ImportJob;

public class ImportJobViewModel
{
    public int Id { get; set; }

    public required string OriginalFileName { get; set; }

    public required JsonDocument MappingProfile { get; set; }

    public int TotalRows { get; set; }

    public int SuccessCount { get; set; }

    public int ErrorCount { get; set; }

    public ImportJobStatus Status { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public bool NotifyOnCompletion { get; set; }
}
