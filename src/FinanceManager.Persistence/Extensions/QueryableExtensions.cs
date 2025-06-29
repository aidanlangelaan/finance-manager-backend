using FinanceManager.Application.Common.Models.Paging;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PagedRequest paging,
        CancellationToken ct = default)
    {
        var count = await query.CountAsync(ct);
        var items = await query
            .Skip(paging.Skip)
            .Take(paging.ResolvedPageSize)
            .ToListAsync(ct);

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = count,
            PageNumber = paging.ResolvedPageNumber,
            PageSize = paging.ResolvedPageSize
        };
    }
}
