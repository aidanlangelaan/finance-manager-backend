namespace FinanceManager.Api.Models;

public class AuthorizationTokenViewModel
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
}