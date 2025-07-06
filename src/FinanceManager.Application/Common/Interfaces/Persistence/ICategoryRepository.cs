using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id, int? userId, CancellationToken ct);
    IQueryable<Category> GetAllAsync(int? userId);
    Task AddAsync(Category category, CancellationToken ct);
    Task UpdateAsync(Category category, CancellationToken ct);
    Task DeleteAsync(Category category, CancellationToken ct);
    Task<Category?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct);
}
