namespace FinanceManager.Api.Models;

public class ResetPasswordViewModel
{
    public string Password { get; init; } = null!;

    public string Token { get; init; } = null!;
}