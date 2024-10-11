namespace FinanceManager.Api.Models;

public class AuthorizationTokenViewModel
{
    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}