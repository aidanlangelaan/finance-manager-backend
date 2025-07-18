using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Accounts.Validators;
using FinanceManager.Domain.Enums;
using Shouldly;

namespace FinanceManager.Application.Tests.Validators;

public class UpdateAccountValidatorTests
{
    private readonly UpdateAccountValidator _validator;

    public UpdateAccountValidatorTests()
    {
        _validator = new UpdateAccountValidator();
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var dto = new UpdateAccountDto { Name = string.Empty, Type = AccountType.Asset };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateAccountDto.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var dto = new UpdateAccountDto { Name = new string('a', 256), Type = AccountType.Asset };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateAccountDto.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenTypeIsInvalid()
    {
        // Arrange
        var dto = new UpdateAccountDto { Name = "Test Account", Type = (AccountType)99 };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateAccountDto.Type));
    }

    [Fact]
    public void ShouldNotHaveError_WhenDtoIsValid()
    {
        // Arrange
        var dto = new UpdateAccountDto { Name = "Test Account", Type = AccountType.Asset };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
