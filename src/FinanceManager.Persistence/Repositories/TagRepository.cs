using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence.Repositories;

public class TagRepository(AppDbContext context) : ITagRepository
{
    public async Task<Tag?> GetByIdAsync(int id, int? userId, CancellationToken ct)
    {
        return await context.Tags
            .Where(a => a.Id == id && a.CreatedById == userId)
            .FirstOrDefaultAsync(ct);
    }

    public IQueryable<Tag> GetAllAsync(int? userId)
    {
        return context.Tags
            .Where(a => a.CreatedById == userId)
            .OrderBy(a => a.Name);
    }

    public async Task AddAsync(Tag tag, CancellationToken ct)
    {
        await context.Tags.AddAsync(tag, ct);
    }

    public Task UpdateAsync(Tag tag, CancellationToken ct)
    {
        context.Tags.Update(tag);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Tag tag, CancellationToken ct)
    {
        context.Tags.Remove(tag);
        return Task.CompletedTask;
    }

    public async Task<Tag?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct) =>
        await context.Tags
            .FirstOrDefaultAsync(a => a.Id == id && a.CreatedById == userId, ct);
}
