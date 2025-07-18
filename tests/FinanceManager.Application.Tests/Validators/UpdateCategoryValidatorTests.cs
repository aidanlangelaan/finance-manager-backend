using FinanceManager.Application.Categories.Dtos;
using FinanceManager.Application.Categories.Validators;
using Shouldly;

namespace FinanceManager.Application.Tests.Validators;

public class UpdateCategoryValidatorTests
{
    private readonly UpdateCategoryValidator _validator;

    public UpdateCategoryValidatorTests()
    {
        _validator = new UpdateCategoryValidator();
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var dto = new UpdateCategoryDto { Name = string.Empty };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateCategoryDto.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var dto = new UpdateCategoryDto { Name = new string('a', 256) };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateCategoryDto.Name));
    }

    [Fact]
    public void ShouldNotHaveError_WhenDtoIsValid()
    {
        // Arrange
        var dto = new UpdateCategoryDto { Name = "Test Category" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
