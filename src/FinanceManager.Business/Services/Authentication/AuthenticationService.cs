using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using FinanceManager.Business.Exceptions;
using FinanceManager.Business.Utils;
using FinanceManager.Data;
using FinanceManager.Data.Enums;
using FinanceManager.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinanceManager.Business.Services;

public class AuthenticationService(
    IConfiguration configuration,
    FinanceManagerDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ILogger<AuthenticationService> logger,
    ISmtpEmailService emailService)
    : IAuthenticationService
{
    private const string SALT_SETTINGS_KEY = "Security:Salt";
    private const string AUTHENTICATION_LOGIN_PROVIDER_SETTINGS_KEY = "Authentication:LoginProvider";

    public async Task<IdentityResult> CreateUser(RegisterUserDTO model)
    {
        try
        {
            var existingUser = await userManager.FindByEmailAsync(model.EmailAddress);
            if (existingUser != null)
            {
                logger.LogWarning("Attempt to register with an existing email: {Email}", model.EmailAddress);
                return IdentityResult.Failed(new IdentityError
                    { Description = "Registration failed. Please try again." });
            }

            var user = new User()
            {
                UserName = model.EmailAddress,
                Email = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmailConfirmed = false,
                LockoutEnabled = true,
            };

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                logger.LogError("Failed to create user: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return result;
            }

            // give all users the user role by default
            var roleName = Roles.User.StringValue();
            if (await roleManager.RoleExistsAsync(roleName))
            {
                var roleResult = await userManager.AddToRoleAsync(user, roleName);
                if (!roleResult.Succeeded)
                {
                    logger.LogError("Failed to add user to role '{Role}': {Errors}", roleName,
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    return roleResult;
                }
            }

            // Generate and store the email confirmation token
            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.SetAuthenticationTokenAsync(user,
                configuration[AUTHENTICATION_LOGIN_PROVIDER_SETTINGS_KEY] ?? TokenOptions.DefaultProvider, "EmailConfirmation", confirmationToken);

            // Generate the confirmation link and send mail
            var confirmationLink = GenerateEmailConfirmationLink(confirmationToken);
            await emailService.SendEmailAsync(user.Email, "Confirm your email address",
                $"Please confirm your email by clicking the following link: {confirmationLink}");

            logger.LogInformation("User registered successfully and confirmation email sent to {Email}", user.Email);

            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred during user registration.");
            return IdentityResult.Failed(new IdentityError
                { Description = "An error occurred while processing your request. Please try again later." });
        }
    }

    public async Task<IdentityResult> ConfirmEmailAddress(ConfirmEmailAddressDTO model)
    {
        // Find the associated user
        var userToken = await context.UserTokens
            .SingleOrDefaultAsync(ut =>
                ut.LoginProvider == (configuration[AUTHENTICATION_LOGIN_PROVIDER_SETTINGS_KEY] ?? TokenOptions.DefaultProvider) &&
                ut.Name == "EmailConfirmation" &&
                ut.Value == model.Token);

        if (userToken == null)
        {
            logger.LogWarning("Invalid or expired email confirmation token.");
            return IdentityResult.Failed(new IdentityError { Description = "Invalid confirmation token." });
        }

        // Retrieve the user using the UserId from the token entry
        var user = await userManager.FindByIdAsync(userToken.UserId.ToString());
        if (user == null)
        {
            logger.LogWarning("User not found for the provided email confirmation token.");
            return IdentityResult.Failed(new IdentityError { Description = "Invalid confirmation token." });
        }

        // Verify the token for the user
        var isTokenValid =
            await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "EmailConfirmation",
                model.Token);
        if (!isTokenValid)
        {
            logger.LogWarning("Token verification failed.");
            return IdentityResult.Failed(new IdentityError { Description = "Invalid or expired confirmation token." });
        }

        // Step 4: Confirm the user's email
        var result = await userManager.ConfirmEmailAsync(user, model.Token);
        if (result.Succeeded)
        {
            logger.LogInformation("Email successfully confirmed for user: {UserId}", user.Id);
        }
        else
        {
            logger.LogError("Email confirmation failed for user: {UserId}", user.Id);
        }

        return result;
    }

    public async Task<IdentityResult> ResetPassword(ResetPasswordDTO model)
    {
        var userToken = await context.UserTokens
            .SingleOrDefaultAsync(ut =>
                ut.LoginProvider == (configuration[AUTHENTICATION_LOGIN_PROVIDER_SETTINGS_KEY] ?? TokenOptions.DefaultProvider) &&
                ut.Name == "ResetPassword" &&
                ut.Value == model.Token);

        if (userToken == null)
        {
            throw new InvalidTokenException("Invalid password reset token.");
        }

        var user = await userManager.FindByIdAsync(userToken.UserId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException("User not found.");
        }

        return await userManager.ResetPasswordAsync(user, model.Token, model.Password);
    }

    public async Task<AuthorizationTokenDTO?> LoginUser(LoginUserDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.EmailAddress);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            throw new UserNotFoundException("User not found.");
        }

        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        await userManager.SetAuthenticationTokenAsync(user,
            configuration[AUTHENTICATION_LOGIN_PROVIDER_SETTINGS_KEY] ?? TokenOptions.DefaultProvider,
            "RefreshToken",
            SecurityUtils.HashValue(refreshToken, configuration[SALT_SETTINGS_KEY] ?? string.Empty));

        return new AuthorizationTokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthorizationTokenDTO?> RefreshAccessToken(RefreshAccessTokenDTO model)
    {
        var hashedRefreshToken =
            SecurityUtils.HashValue(model.RefreshToken, configuration[SALT_SETTINGS_KEY] ?? string.Empty);

        var userToken = await context.UserTokens
            .SingleOrDefaultAsync(ut =>
                ut.LoginProvider == (configuration[AUTHENTICATION_LOGIN_PROVIDER_SETTINGS_KEY] ?? TokenOptions.DefaultProvider) &&
                ut.Name == "RefreshToken" &&
                ut.Value == hashedRefreshToken);

        if (userToken == null)
        {
            throw new InvalidTokenException("Invalid refresh token.");
        }

        var user = await userManager.FindByIdAsync(userToken.UserId.ToString());
        if (user == null)
        {
            throw new UserNotFoundException("User not found.");
        }

        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        await userManager.SetAuthenticationTokenAsync(user,
            configuration[AUTHENTICATION_LOGIN_PROVIDER_SETTINGS_KEY] ?? TokenOptions.DefaultProvider,
            "RefreshToken",
            SecurityUtils.HashValue(refreshToken, configuration[SALT_SETTINGS_KEY] ?? string.Empty));

        return new AuthorizationTokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private string GenerateEmailConfirmationLink(string token)
    {
        var baseUrl = configuration["ApplicationSettings:FrontendBaseUrl"];
        var confirmationPath = configuration["ApplicationSettings:EmailConfirmationPath"];
        var encodedToken = Uri.EscapeDataString(token);
        return $"{baseUrl}{confirmationPath}?token={encodedToken}";
    }

    private string GenerateAccessToken(User user)
    {
        var tokenHandler = new JsonWebTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Authentication:Secret"] ??
                                          throw new ConfigurationException("Authentication Secret can't be null"));
        var issuingOnAt = DateTimeOffset.UtcNow;
        var expiringOnAt =
            issuingOnAt.AddMinutes(Convert.ToDouble(configuration["Authentication:AccessTokenExpirationInMinutes"]));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["Authentication:ValidIssuer"],
            Audience = configuration["Authentication:ValidAudience"],
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
            ]),
            Expires = expiringOnAt.UtcDateTime,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.CreateToken(tokenDescriptor);
    }

    private static string GenerateRefreshToken()
    {
        var refreshToken = Guid.NewGuid().ToString();
        return refreshToken;
    }
}