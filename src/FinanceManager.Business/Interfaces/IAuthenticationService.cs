using System.Threading.Tasks;
using FinanceManager.Business.Services.Models;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinanceManager.Business.Interfaces;

public interface IAuthenticationService
{
    public Task<AuthorizationTokenDTO> LoginUser(LoginUserDTO model);
    public Task<bool> RegisterUser(RegisterUserDTO model);
}