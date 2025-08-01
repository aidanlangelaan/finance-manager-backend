namespace FinanceManager.Application.ImportJobs.Dtos;

public class ImportErrorResponseDto
{
    public int Id { get; set; }

    public int RowNumber { get; set; }

    public required string RawData { get; set; }

    public required string ErrorMessage { get; set; }
}
