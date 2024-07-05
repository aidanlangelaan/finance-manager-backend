namespace FinanceManager.Business.Services.Models;

public class AuthorizationTokenDTO
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}