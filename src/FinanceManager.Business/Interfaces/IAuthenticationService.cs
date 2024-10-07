using FinanceManager.Business.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceManager.Business.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> CreateUser(RegisterUserDTO model);

    Task<IdentityResult> ConfirmEmailAddress(ConfirmEmailAddressDTO model);

    Task<IdentityResult> ResetPassword(ResetPasswordDTO model);
    
    Task<AuthorizationTokenDTO?> LoginUser(LoginUserDTO model);
    
    Task<AuthorizationTokenDTO?> RefreshAccessToken(RefreshAccessTokenDTO model);
}