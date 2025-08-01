using System.Text.Json;

namespace FinanceManager.Application.ImportJobs.Dtos;

public class CreateImportJobDto
{
    public required string OriginalFileName { get; init; }

    public required JsonDocument MappingProfile { get; init; }

    public bool NotifyOnCompletion { get; init; }
}
