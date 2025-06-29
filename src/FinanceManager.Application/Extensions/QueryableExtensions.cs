using FinanceManager.Application.Common.Models.Paging;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Application.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<TDestination>> ToPagedResultAsync<TSource, TDestination>(
        this IQueryable<TSource> query,
        PagedRequest paging,
        Func<TSource, TDestination>? converter = null,
        CancellationToken ct = default)
    {
        var count = await query.CountAsync(ct);
        var items = await query
            .Skip(paging.Skip)
            .Take(paging.ResolvedPageSize)
            .ToListAsync(ct);

        var convertedItems = converter == null ? (List<TDestination>)(object)items : items.Select(converter).ToList();

        return new PagedResult<TDestination>
        {
            Items = convertedItems,
            TotalCount = count,
            PageNumber = paging.ResolvedPageNumber,
            PageSize = paging.ResolvedPageSize
        };
    }
}
