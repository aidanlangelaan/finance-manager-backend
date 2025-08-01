namespace FinanceManager.Application.ImportJobs.Dtos;

public class ImportJobDetailsResponseDto : ImportJobResponseDto
{
    public IEnumerable<ImportErrorResponseDto> Errors { get; set; } = [];
}
