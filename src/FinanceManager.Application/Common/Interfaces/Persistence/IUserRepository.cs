using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetUserByKeycloakIdAsync(Guid keycloakId, CancellationToken ct = default);
    Task AddUserAsync(User user, CancellationToken ct = default);
    Task UpdateUserAsync(User user, CancellationToken ct = default);
}
