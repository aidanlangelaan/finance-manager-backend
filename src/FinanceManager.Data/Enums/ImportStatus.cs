using FinanceManager.Data.Utils;

namespace FinanceManager.Data.Enums;

public enum ImportStatus
{
    [StringValue("Uploaded")]
    Uploaded = 1,
    
    [StringValue("Processing")]
    Processing = 2,
    
    [StringValue("Processed")]
    Processed = 3,
        
    [StringValue("Failed")]
    Failed = 4
}

