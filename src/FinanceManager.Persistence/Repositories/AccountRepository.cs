using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Domain.Entities;
using FinanceManager.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence.Repositories;

public class AccountRepository(AppDbContext context) : IAccountRepository
{
    public async Task<AccountResponseDto?> GetByIdAsync(int id, int? userId, CancellationToken ct)
    {
        return await context.Accounts
            .Where(a => a.Id == id && a.CreatedById == userId)
            .Select(a => new AccountResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Type = a.Type,
                CurrentBalance = a.CurrentBalance,
                IncludedInNetWorth = a.IncludedInNetWorth,
                CanTransferFrom = a.CanTransferFrom,
                CanTransferTo = a.CanTransferTo
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PagedResult<AccountResponseDto>> GetAllAsync(int? userId, PagedRequest paging,
        CancellationToken ct)
    {
        var query = context.Accounts
            .Where(a => a.CreatedById == userId)
            .OrderBy(a => a.Name)
            .Select(a => new AccountResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Type = a.Type,
                CurrentBalance = a.CurrentBalance,
                IncludedInNetWorth = a.IncludedInNetWorth,
                CanTransferFrom = a.CanTransferFrom,
                CanTransferTo = a.CanTransferTo
            });

        return await query.ToPagedResultAsync(paging, ct);
    }

    public Task AddAsync(Account account, CancellationToken ct)
    {
        context.Accounts.Add(account);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Account account, CancellationToken ct)
    {
        context.Accounts.Update(account);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Account account, CancellationToken ct)
    {
        context.Accounts.Remove(account);
        return Task.CompletedTask;
    }

    public async Task<Account?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct) =>
        await context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id && a.CreatedById == userId, ct);
}
