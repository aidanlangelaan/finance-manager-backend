using FinanceManager.Application.Interfaces;

namespace FinanceManager.TestUtilities.Auth;

public class TestCurrentUserService : ICurrentUserService
{
    public Guid? KeycloakId { get; set; } = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
    public string? DisplayName { get; set; } = "Test User";
    public string? Email { get; set; } = "test@user.local";
    public bool IsAuthenticated { get; set; } = true;
}