using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using FinanceManager.Application.Interfaces;
using FinanceManager.Domain.Entities;
using FinanceManager.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Services;

public class UserService(AppDbContext dbContext) : IUserService
{
    public async Task<User> GetOrCreateUserAsync(Guid keycloakId, string? preferredUsername, string? email)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.KeycloakId == keycloakId);

        if (user == null)
        {
            if (string.IsNullOrWhiteSpace(preferredUsername))
            {
                throw new InvalidOperationException("Preferred username is required for user creation.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidOperationException("Email is required for user creation.");
            }
            
            user = new User
            {
                KeycloakId = keycloakId,
                DisplayName = preferredUsername,
                Email = email
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            var updated = false;
            if (!string.IsNullOrEmpty(preferredUsername) && user.DisplayName != preferredUsername)
            {
                user.DisplayName = preferredUsername;
                updated = true;
            }

            if (!string.IsNullOrEmpty(email) && user.Email != email)
            {
                user.Email = email;
                updated = true;
            }

            if (updated)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        return user;
    }
}