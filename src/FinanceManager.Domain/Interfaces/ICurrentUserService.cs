namespace FinanceManager.Domain.Interfaces;

public interface ICurrentUserService
{
    public bool IsAuthenticated { get; }
    
    public string? UserId { get; }
    
    public string? UserName { get; }

    public string? UserEmail { get; }
}