using FinanceManager.Application.Tags.Dtos;
using FinanceManager.Application.Tags.Validators;
using Shouldly;

namespace FinanceManager.Application.Tests.Validators;

public class CreateTagValidatorTests
{
    private readonly CreateTagValidator _validator;

    public CreateTagValidatorTests()
    {
        _validator = new CreateTagValidator();
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var dto = new CreateTagDto { Name = string.Empty };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateTagDto.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var dto = new CreateTagDto { Name = new string('a', 256) };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateTagDto.Name));
    }

    [Fact]
    public void ShouldNotHaveError_WhenDtoIsValid()
    {
        // Arrange
        var dto = new CreateTagDto { Name = "Test Tag" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
