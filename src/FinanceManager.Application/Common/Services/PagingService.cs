using FinanceManager.Application.Common.Interfaces.Paging;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Application.Extensions;

namespace FinanceManager.Application.Common.Services;

public class PagingService : IPagingService
{
    public async Task<PagedResult<TDestination>> ToPagedResultAsync<TSource, TDestination>(
        IQueryable<TSource> query,
        PagedRequest paging,
        Func<TSource, TDestination>? converter = null,
        CancellationToken ct = default) where TSource : class
    {
        return await query.ToPagedResultAsync(paging, converter, ct);
    }
}
