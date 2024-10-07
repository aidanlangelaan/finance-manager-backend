namespace FinanceManager.Business.Services.Models;

public class ResetPasswordDTO
{
    public string Token { get; init; } = null!;
    
    public string Password { get; init; } = null!;
}