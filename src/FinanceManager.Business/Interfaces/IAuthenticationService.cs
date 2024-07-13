using System.Threading.Tasks;
using FinanceManager.Business.Services.Models;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinanceManager.Business.Interfaces;

public interface IAuthenticationService
{
    Task<bool> RegisterUser(RegisterUserDTO model);
    Task<AuthorizationTokenDTO?> LoginUser(LoginUserDTO model);
    Task<AuthorizationTokenDTO?> RefreshAccessToken(RefreshAccessTokenDTO model);
}