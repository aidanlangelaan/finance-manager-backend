namespace FinanceManager.Application.Common.Interfaces;

public interface IUserProvisioningService
{
    Task<Domain.Entities.User> GetOrCreateUserAsync(Guid keycloakId, string? name, string? email);
}
