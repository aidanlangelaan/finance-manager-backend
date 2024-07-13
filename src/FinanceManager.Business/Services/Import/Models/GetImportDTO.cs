using FinanceManager.Data.Enums;

namespace FinanceManager.Business.Services.Models;

public class GetImportDTO
{
    public int Id { get; set; }

    public string OriginalFileName { get; set; }

    public BankType BankType { get; set; }
    
    public ImportStatus Status { get; set; }

    public DateTime CreatedOnAt { get; set; }
    
    public DateTime UpdatedOnAt { get; set; }
}