using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using FinanceManager.Business.Services.Models;

namespace FinanceManager.Business.Interfaces;

public interface IAuthenticationService
{
    public Task<JwtSecurityToken> LoginUser(LoginUserDTO model);
    public Task<bool> RegisterUser(RegisterUserDTO model);
}