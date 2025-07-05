using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Accounts.Validators;
using FinanceManager.Domain.Enums;
using Shouldly;

namespace FinanceManager.Application.Tests.Validators;

public class CreateTransactionValidatorTests
{
    private readonly CreateAccountValidator _validator;

    public CreateTransactionValidatorTests()
    {
        _validator = new CreateAccountValidator();
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var dto = new CreateAccountDto { Name = string.Empty, Type = AccountType.Asset, CurrentBalance = 0 };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateAccountDto.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var dto = new CreateAccountDto { Name = new string('a', 101), Type = AccountType.Asset, CurrentBalance = 0 };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateAccountDto.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenTypeIsInvalid()
    {
        // Arrange
        var dto = new CreateAccountDto { Name = "Test Account", Type = (AccountType)99, CurrentBalance = 0 };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateAccountDto.Type));
    }

    [Fact]
    public void ShouldHaveError_WhenCurrentBalanceIsNegative()
    {
        // Arrange
        var dto = new CreateAccountDto { Name = "Test Account", Type = AccountType.Asset, CurrentBalance = -1 };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(CreateAccountDto.CurrentBalance));
    }

    [Fact]
    public void ShouldNotHaveError_WhenDtoIsValid()
    {
        // Arrange
        var dto = new CreateAccountDto { Name = "Test Account", Type = AccountType.Asset, CurrentBalance = 100 };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
