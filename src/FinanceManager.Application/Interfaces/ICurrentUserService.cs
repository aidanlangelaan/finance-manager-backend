namespace FinanceManager.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? KeycloakId { get; }
    string? DisplayName { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}