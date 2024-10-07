namespace FinanceManager.Api.Models;

public class ResetPasswordViewModel
{
    public string Password { get; set; } = null!;

    public string Token { get; set; } = null!;
}