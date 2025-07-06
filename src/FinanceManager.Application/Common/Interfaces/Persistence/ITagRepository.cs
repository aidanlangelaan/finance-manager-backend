using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(int id, int? userId, CancellationToken ct);
    IQueryable<Tag> GetAllAsync(int? userId);
    Task AddAsync(Tag tag, CancellationToken ct);
    Task UpdateAsync(Tag tag, CancellationToken ct);
    Task DeleteAsync(Tag tag, CancellationToken ct);
    Task<Tag?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct);
}
