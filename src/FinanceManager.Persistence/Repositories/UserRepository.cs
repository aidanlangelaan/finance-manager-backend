using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetUserByKeycloakIdAsync(Guid keycloakId, CancellationToken ct = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.KeycloakId == keycloakId, ct);
    }

    public async Task AddUserAsync(User user, CancellationToken ct = default)
    {
        await context.Users.AddAsync(user, ct);
    }

    public Task UpdateUserAsync(User user, CancellationToken ct = default)
    {
        context.Users.Update(user);
        return Task.CompletedTask;
    }
}
