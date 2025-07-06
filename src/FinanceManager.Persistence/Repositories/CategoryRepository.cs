using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(int id, int? userId, CancellationToken ct)
    {
        return await context.Categories
            .Where(a => a.Id == id && a.CreatedById == userId)
            .FirstOrDefaultAsync(ct);
    }

    public IQueryable<Category> GetAllAsync(int? userId)
    {
        return context.Categories
            .Where(a => a.CreatedById == userId)
            .OrderBy(a => a.Name);
    }

    public async Task AddAsync(Category category, CancellationToken ct)
    {
        await context.Categories.AddAsync(category, ct);
    }

    public Task UpdateAsync(Category category, CancellationToken ct)
    {
        context.Categories.Update(category);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Category category, CancellationToken ct)
    {
        context.Categories.Remove(category);
        return Task.CompletedTask;
    }

    public async Task<Category?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct) =>
        await context.Categories
            .FirstOrDefaultAsync(a => a.Id == id && a.CreatedById == userId, ct);
}
