using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence.Repositories;

public class ImportJobRepository(AppDbContext context) : IImportJobRepository
{
    public async Task<ImportJob?> GetByIdAsync(int id, int? userId, CancellationToken ct)
    {
        return await context.ImportJobs
            .Include(i => i.Errors)
            .Where(a => a.Id == id && a.CreatedById == userId)
            .FirstOrDefaultAsync(ct);
    }

    public IQueryable<ImportJob> GetAllAsync(int? userId)
    {
        return context.ImportJobs
            .Where(a => a.CreatedById == userId);
    }

    public async Task AddAsync(ImportJob importJob, CancellationToken ct)
    {
        await context.ImportJobs.AddAsync(importJob, ct);
    }

    public Task UpdateAsync(ImportJob importJob, CancellationToken ct)
    {
        context.ImportJobs.Update(importJob);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ImportJob importJob, CancellationToken ct)
    {
        context.ImportJobs.Remove(importJob);
        return Task.CompletedTask;
    }

    public async Task<ImportJob?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct) =>
        await context.ImportJobs
            .FirstOrDefaultAsync(a => a.Id == id && a.CreatedById == userId, ct);
}
