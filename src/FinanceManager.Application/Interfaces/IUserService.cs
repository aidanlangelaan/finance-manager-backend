using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Interfaces;

public interface IUserService
{
    Task<User> GetOrCreateUserAsync(Guid keycloakId, string? preferredUsername, string? email);
}