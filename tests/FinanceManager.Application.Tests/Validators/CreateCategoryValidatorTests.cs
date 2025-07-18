using FinanceManager.Application.Categories.Dtos;
using FinanceManager.Application.Categories.Validators;
using Shouldly;

namespace FinanceManager.Application.Tests.Validators;

public class CreateCategoryValidatorTests
{
    private readonly CreateCategoryValidator _validator;

    public CreateCategoryValidatorTests()
    {
        _validator = new CreateCategoryValidator();
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var dto = new CreateCategoryDto { Name = string.Empty };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateCategoryDto.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var dto = new CreateCategoryDto { Name = new string('a', 256) };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateCategoryDto.Name));
    }

    [Fact]
    public void ShouldNotHaveError_WhenDtoIsValid()
    {
        // Arrange
        var dto = new CreateCategoryDto { Name = "Test Category" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
