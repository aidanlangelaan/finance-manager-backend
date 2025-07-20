using System.Text.Json;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Domain.Entities;

public class ImportJob : AuditableEntity
{
    public string OriginalFileName { get; set; } = string.Empty;

    public string StoredFileName { get; set; } = string.Empty;

    public JsonDocument MappingProfile { get; set; } = null!;

    public int TotalRows { get; set; }

    public int SuccessCount { get; set; }

    public int ErrorCount { get; set; }

    public ImportJobStatus Status { get; set; } = ImportJobStatus.Pending;

    public DateTime StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public bool NotifyOnCompletion { get; set; }

    // Relationships
    public ICollection<ImportError> Errors { get; set; } = new List<ImportError>();
}
