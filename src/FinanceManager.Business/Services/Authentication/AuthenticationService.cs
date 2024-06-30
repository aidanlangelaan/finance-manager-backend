using AutoMapper;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data;
using FinanceManager.Data.Constants;
using FinanceManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Business.Services;

public class AuthenticationService(
    FinanceManagerDbContext context,
    IMapper mapper,
    IConfiguration configuration,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
    : IAuthenticationService
{
    public async Task<JwtSecurityToken> LoginUser(LoginUserDTO model)
    {
        var user = await userManager.FindByEmailAsync(model.EmailAddress);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
        {
            return null;
        }

        var issuingOnAt = DateTime.UtcNow;
        var expiringOnAt = issuingOnAt.AddHours(3);

        var authClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub,
                user.UserName ?? throw new InvalidOperationException("Username can't be empty")),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, issuingOnAt.ToString(CultureInfo.InvariantCulture)),
            new(JwtRegisteredClaimNames.Exp, issuingOnAt.ToString(CultureInfo.InvariantCulture)),
        };

        var userRoles = await userManager.GetRolesAsync(user);
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Authentication:Secret"] ??
                                   throw new InvalidOperationException("Secret can't be empty")));
        
        var token = new JwtSecurityToken(
            issuer: configuration["Authentication:ValidIssuer"],
            audience: configuration["Authentication:ValidAudience"],
            expires: expiringOnAt,
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
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