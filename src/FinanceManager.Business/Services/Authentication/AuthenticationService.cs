using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinanceManager.Business.Utils;
using FinanceManager.Data;
using FinanceManager.Data.Enums;
using FinanceManager.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinanceManager.Business.Services;

public class AuthenticationService(
    IConfiguration configuration,
    FinanceManagerDbContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : IAuthenticationService
{
    private const string SALT_SETTINGS_KEY = "Security:Salt";

    public async Task<bool> RegisterUser(RegisterUserDTO model)
    {
        var existingUser = await userManager.FindByEmailAsync(model.EmailAddress);
        if (existingUser != null)
        {
            return false;
        }

        var user = new User()
        {
            Email = model.EmailAddress,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.EmailAddress,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return false;
        }

        // give all users the user role by default
        if (await roleManager.RoleExistsAsync(Roles.User.StringValue()))
        {
            await userManager.AddToRoleAsync(user, Roles.User.StringValue());
        }

        return true;
    }

    public async Task<AuthorizationTokenDTO?> LoginUser(LoginUserDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.EmailAddress);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            return null;
        }

        var accessToken = await GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        var existingToken = await context.UserTokens
            .FirstOrDefaultAsync(t => t.LoginProvider == configuration["Authentication:LoginProvider"]
                                      && t.UserId == user.Id
                                      && t.Name == user.NormalizedUserName);
        if (existingToken != null)
        {
            context.UserTokens.Remove(existingToken);
        }

        await context.UserTokens.AddAsync(new UserToken
        {
            UserId = user.Id,
            LoginProvider = configuration["Authentication:LoginProvider"] ?? string.Empty,
            Name = user.NormalizedEmail!,
            Value = SecurityUtils.HashValue(accessToken, configuration[SALT_SETTINGS_KEY]),
            RefreshToken = SecurityUtils.HashValue(refreshToken, configuration[SALT_SETTINGS_KEY]),
            RefreshTokenExpiresOnAt =
                DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(configuration["Authentication:RefreshTokenExpirationInMinutes"]))
        });

        await context.SaveChangesAsync();

        return new AuthorizationTokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthorizationTokenDTO?> RefreshAccessToken(RefreshAccessTokenDTO model)
    {
        string identity;

        try
        {
            identity = await GetIdentityFromExpiredAccessToken(model.AccessToken);
        }
        catch (SecurityTokenException)
        {
            return null;
        }

        var userToken = await context.UserTokens
            .FirstOrDefaultAsync(t =>
                t.Value == SecurityUtils.HashValue(model.AccessToken, configuration[SALT_SETTINGS_KEY])
                && t.RefreshToken == SecurityUtils.HashValue(model.RefreshToken, configuration[SALT_SETTINGS_KEY])
                && t.LoginProvider == configuration["Authentication:LoginProvider"]
                && t.Name == identity);
        if (userToken == null)
        {
            return null;
        }

        if (userToken.RefreshTokenExpiresOnAt < DateTime.UtcNow)
        {
            return null;
        }

        var user = await context.Users.FindAsync(userToken.UserId);
        var accessToken = await GenerateAccessToken(user!);
        var refreshToken = GenerateRefreshToken();

        // hash the tokens
        userToken.Value = SecurityUtils.HashValue(accessToken, configuration[SALT_SETTINGS_KEY]);
        userToken.RefreshToken = SecurityUtils.HashValue(refreshToken, configuration[SALT_SETTINGS_KEY]);
        userToken.RefreshTokenExpiresOnAt =
            DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(configuration["Authentication:RefreshTokenExpirationInMinutes"]));

        await context.SaveChangesAsync();

        return new AuthorizationTokenDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private async Task<string> GenerateAccessToken(User user)
    {
        var issuingOnAt = DateTimeOffset.UtcNow;
        var expiringOnAt =
            issuingOnAt.AddMinutes(Convert.ToDouble(configuration["Authentication:AccessTokenExpirationInMinutes"]));

        var authClaims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] =
                user.NormalizedUserName ?? throw new InvalidOperationException("Username can't be empty"),
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
            [JwtRegisteredClaimNames.Iat] = issuingOnAt.ToUnixTimeSeconds().ToString(),
            [JwtRegisteredClaimNames.Exp] = expiringOnAt.ToUnixTimeSeconds().ToString()
        };

        var userRoles = await userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            authClaims.Add(ClaimTypes.Role, role);
        }

        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Authentication:Secret"] ??
                                   throw new InvalidOperationException("Secret can't be empty")));

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["Authentication:ValidIssuer"],
            Audience = configuration["Authentication:ValidAudience"],
            Expires = expiringOnAt.UtcDateTime,
            Claims = authClaims,
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        };

        var handler = new JsonWebTokenHandler
        {
            SetDefaultTimesOnTokenCreation = false
        };

        return handler.CreateToken(descriptor);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task<string> GetIdentityFromExpiredAccessToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = configuration["Authentication:ValidAudience"],
            ValidateIssuer = true,
            ValidIssuer = configuration["Authentication:ValidIssuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration["Authentication:Secret"] ??
                throw new InvalidOperationException("Secret can't be empty"))),
        };

        var tokenHandler = new JsonWebTokenHandler();
        var result = await tokenHandler.ValidateTokenAsync(accessToken, tokenValidationParameters);
        if (!result.IsValid)
        {
            throw new SecurityTokenException("Invalid token");
        }

        return result.Claims[JwtRegisteredClaimNames.Sub].ToString()!;
    }
}