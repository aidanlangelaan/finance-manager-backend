using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Application.Extensions;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace FinanceManager.Application.Tests.Extensions;

public class QueryableExtensionsTests
{
    // [Fact]
    // public void ToPagedResult_Should_Return_Correct_PagedResult()
    // {
    //     // Arrange
    //     var data = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.AsQueryable();
    //     var paging = new PagedRequest { PageNumber = 2, PageSize = 3 };
    //
    //     // Act
    //     var result = data.ToPagedResult<int, int>(paging);
    //
    //     // Assert
    //     Assert.NotNull(result);
    //     Assert.Equal(3, result.Items.Count);
    //     Assert.Equal(4, result.Items.First());
    //     Assert.Equal(10, result.TotalCount);
    //     Assert.Equal(2, result.PageNumber);
    //     Assert.Equal(3, result.PageSize);
    //     Assert.Equal(4, result.TotalPages);
    // }
    //
    // [Fact]
    // public void ToPagedResult_With_Converter_Should_Return_Correct_PagedResult()
    // {
    //     // Arrange
    //     var data = new List<int> { 1, 2, 3, 4, 5 }.AsQueryable();
    //     var paging = new PagedRequest { PageNumber = 1, PageSize = 2 };
    //     Func<int, string> converter = i => (i * 2).ToString();
    //
    //     // Act
    //     var result = data.ToPagedResult(paging, converter);
    //
    //     // Assert
    //     Assert.NotNull(result);
    //     Assert.Equal(2, result.Items.Count);
    //     Assert.Equal("2", result.Items.First());
    //     Assert.Equal("4", result.Items.Last());
    //     Assert.Equal(5, result.TotalCount);
    //     Assert.Equal(1, result.PageNumber);
    //     Assert.Equal(2, result.PageSize);
    //     Assert.Equal(3, result.TotalPages);
    // }
}
