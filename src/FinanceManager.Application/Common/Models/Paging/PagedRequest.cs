namespace FinanceManager.Application.Common.Models.Paging;

public class PagedRequest
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }

    public int ResolvedPageNumber => PageNumber.GetValueOrDefault(1);
    public int ResolvedPageSize => PageSize.GetValueOrDefault(20);

    public int Skip => (ResolvedPageNumber - 1) * ResolvedPageSize;
}
