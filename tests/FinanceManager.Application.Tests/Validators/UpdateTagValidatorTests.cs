using FinanceManager.Application.Tags.Dtos;
using FinanceManager.Application.Tags.Validators;
using Shouldly;

namespace FinanceManager.Application.Tests.Validators;

public class UpdateTagValidatorTests
{
    private readonly UpdateTagValidator _validator;

    public UpdateTagValidatorTests()
    {
        _validator = new UpdateTagValidator();
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var dto = new UpdateTagDto { Name = string.Empty };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateTagDto.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var dto = new UpdateTagDto { Name = new string('a', 256) };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateTagDto.Name));
    }

    [Fact]
    public void ShouldNotHaveError_WhenDtoIsValid()
    {
        // Arrange
        var dto = new UpdateTagDto { Name = "Test Tag" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
