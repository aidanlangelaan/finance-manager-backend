using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Application.Common.Validators;
using Shouldly;

namespace FinanceManager.Application.Tests.Common.Validators;

public class PagedRequestValidatorTests
{
    private readonly PagedRequestValidator _validator;

    public PagedRequestValidatorTests()
    {
        _validator = new PagedRequestValidator();
    }

    [Fact]
    public void ShouldHaveError_WhenPageNumberIsLessThanOne()
    {
        // Arrange
        var request = new PagedRequest { PageNumber = 0 };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(PagedRequest.PageNumber));
    }

    [Fact]
    public void ShouldHaveError_WhenPageSizeIsLessThanOne()
    {
        // Arrange
        var request = new PagedRequest { PageSize = 0 };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(PagedRequest.PageSize));
    }

    [Fact]
    public void ShouldHaveError_WhenPageSizeIsGreaterThanOneHundred()
    {
        // Arrange
        var request = new PagedRequest { PageSize = 101 };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(PagedRequest.PageSize));
    }

    [Fact]
    public void ShouldNotHaveError_WhenRequestIsValid()
    {
        // Arrange
        var request = new PagedRequest { PageNumber = 1, PageSize = 10 };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void ShouldNotHaveError_WhenPageNumberAndPageSizeAreNull()
    {
        // Arrange
        var request = new PagedRequest { PageNumber = null, PageSize = null };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
