using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface IImportJobRepository
{
    Task<ImportJob?> GetByIdAsync(int id, int? userId, CancellationToken ct);
    IQueryable<ImportJob> GetAllAsync(int? userId);
    Task AddAsync(ImportJob importJob, CancellationToken ct);
    Task UpdateAsync(ImportJob importJob, CancellationToken ct);
    Task DeleteAsync(ImportJob importJob, CancellationToken ct);
    Task<ImportJob?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct);
}
