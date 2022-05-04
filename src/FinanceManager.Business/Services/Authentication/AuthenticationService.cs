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
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly FinanceManagerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthenticationService(FinanceManagerDbContext context, IMapper mapper, IConfiguration configuration, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<JwtSecurityToken> LoginUser(LoginUserDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return null;
            }

            var issuingOnAt = DateTime.UtcNow;
            var expiringOnAt = issuingOnAt.AddHours(3);

            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, issuingOnAt.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, issuingOnAt.ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["Authentication:ValidIssuer"],
                    audience: _configuration["Authentication:ValidAudience"],
                    expires: expiringOnAt,
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        public async Task<bool> RegisterUser(RegisterUserDTO model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (existingUser != null)
            {
                return false;
            }

            User user = new User()
            {
                Email = model.EmailAddress,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.EmailAddress,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return false;
            }

            if (!await _roleManager.RoleExistsAsync(RoleConstants.Admin))
                await _roleManager.CreateAsync(new Role(RoleConstants.Admin));
            if (!await _roleManager.RoleExistsAsync(RoleConstants.User))
                await _roleManager.CreateAsync(new Role(RoleConstants.User));

            if (await _roleManager.RoleExistsAsync(RoleConstants.Admin))
            {
                await _userManager.AddToRoleAsync(user, RoleConstants.Admin);
            }

            return true;
        }
    }
}
