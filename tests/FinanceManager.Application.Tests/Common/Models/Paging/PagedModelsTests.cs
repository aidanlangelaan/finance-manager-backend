using FinanceManager.Application.Common.Models.Paging;
using Shouldly;

namespace FinanceManager.Application.Tests.Common.Models.Paging;

public class PagedModelsTests
{
    [Fact]
    public void PagedRequest_ResolvedPageNumber_ShouldReturnCorrectValue()
    {
        // Arrange
        var request1 = new PagedRequest { PageNumber = 5 };
        var request2 = new PagedRequest { PageNumber = null };

        // Assert
        request1.ResolvedPageNumber.ShouldBe(5);
        request2.ResolvedPageNumber.ShouldBe(1);
    }

    [Fact]
    public void PagedRequest_ResolvedPageSize_ShouldReturnCorrectValue()
    {
        // Arrange
        var request1 = new PagedRequest { PageSize = 15 };
        var request2 = new PagedRequest { PageSize = null };

        // Assert
        request1.ResolvedPageSize.ShouldBe(15);
        request2.ResolvedPageSize.ShouldBe(20);
    }

    [Fact]
    public void PagedRequest_Skip_ShouldReturnCorrectValue()
    {
        // Arrange
        var request1 = new PagedRequest { PageNumber = 2, PageSize = 10 };
        var request2 = new PagedRequest { PageNumber = 1, PageSize = 5 };
        var request3 = new PagedRequest { PageNumber = null, PageSize = null };

        // Assert
        request1.Skip.ShouldBe(10);
        request2.Skip.ShouldBe(0);
        request3.Skip.ShouldBe(0);
    }

    [Fact]
    public void PagedResult_TotalPages_ShouldReturnCorrectValue()
    {
        // Arrange
        var result1 = new PagedResult<int> { TotalCount = 10, PageSize = 3, Items = new List<int>() };
        var result2 = new PagedResult<int> { TotalCount = 0, PageSize = 10, Items = new List<int>() };
        var result3 = new PagedResult<int> { TotalCount = 10, PageSize = 10, Items = new List<int>() };

        // Assert
        result1.TotalPages.ShouldBe(4);
        result2.TotalPages.ShouldBe(0);
        result3.TotalPages.ShouldBe(1);
    }
}
