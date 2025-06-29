namespace FinanceManager.Application.Common.Models.Paging;

public class PagedResult<T>
{
    public required IReadOnlyCollection<T> Items { get; init; }
    public required int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
