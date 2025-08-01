namespace FinanceManager.Application.ImportJobs.Dtos;

public class ImportJobDetailResponseDto : ImportJobResponseDto
{
    public IEnumerable<ImportErrorResponseDto> Errors { get; set; } = [];
}
