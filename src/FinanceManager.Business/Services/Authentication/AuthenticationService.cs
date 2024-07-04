using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data.Constants;
using FinanceManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FinanceManager.Business.Services;

public class AuthenticationService(
    IConfiguration configuration,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : IAuthenticationService
{
    public async Task<string> LoginUser(LoginUserDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.EmailAddress);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            return null;
        }

        var issuingOnAt = DateTimeOffset.UtcNow;
        var expiringOnAt = issuingOnAt.AddHours(3);

        var authClaims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] = user.UserName ?? throw new InvalidOperationException("Username can't be empty"),
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

        if (!await roleManager.RoleExistsAsync(RoleConstants.Admin))
            await roleManager.CreateAsync(new Role(RoleConstants.Admin));
        if (!await roleManager.RoleExistsAsync(RoleConstants.User))
            await roleManager.CreateAsync(new Role(RoleConstants.User));

        // give all users the user role by default
        if (await roleManager.RoleExistsAsync(RoleConstants.User))
        {
            await userManager.AddToRoleAsync(user, RoleConstants.User);
        }

        return true;
    }
}