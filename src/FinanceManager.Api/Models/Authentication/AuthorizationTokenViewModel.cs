using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace FinanceManager.Api.Models;

public class AuthorizationTokenViewModel(string token)
{
    public string AccessToken { get; init; } = token;
    public string RefreshToken { get; init; }
}