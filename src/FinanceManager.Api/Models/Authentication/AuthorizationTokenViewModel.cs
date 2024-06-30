using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace FinanceManager.Api.Models;

public class AuthorizationTokenViewModel(SecurityToken token)
{
    public string AccessToken { get; init; } = new JwtSecurityTokenHandler().WriteToken(token);
    public string RefreshToken { get; init; }
}