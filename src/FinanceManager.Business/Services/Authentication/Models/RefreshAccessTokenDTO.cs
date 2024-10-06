namespace FinanceManager.Business.Services.Models;

public class RefreshAccessTokenDTO
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
}