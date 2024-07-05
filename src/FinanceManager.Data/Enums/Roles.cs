using FinanceManager.Data.Utils;

namespace FinanceManager.Data.Enums;

public enum Roles
{
    [StringValue("SystemAdmin")]
    SystemAdmin = 1,
    
    [StringValue("Admin")]
    Admin = 2,
    
    [StringValue("User")]
    User = 3
}

