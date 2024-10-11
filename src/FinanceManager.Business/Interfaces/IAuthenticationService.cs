using FinanceManager.Business.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace FinanceManager.Business.Interfaces;

public interface IAuthenticationService
{
    Task<IdentityResult> CreateUser(RegisterUserDTO model);

    Task<IdentityResult> ResendEmailConfirmation(ResendEmailConfirmationDTO model);
    
    Task<IdentityResult> ConfirmEmailAddress(ConfirmEmailAddressDTO model);

    Task<IdentityResult> ForgotPassword(ForgotPasswordDTO model);
    
    Task<IdentityResult> ResetPassword(ResetPasswordDTO model);
    
    // Task<IdentityResult> ChangePassword(ChangePasswordDTO model);
    
    Task<AuthorizationTokenDTO?> LoginUser(LoginUserDTO model);
    
    Task<AuthorizationTokenDTO?> RefreshAccessToken(RefreshAccessTokenDTO model);

    // Task LogoutUser(string userId);

    // Task RevokeRefreshToken(RevokeRefreshTokenDTO model);
}