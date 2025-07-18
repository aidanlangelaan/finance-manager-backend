using FinanceManager.Application.Transactions.Dtos;
using FinanceManager.Application.Transactions.Validators;
using Shouldly;

namespace FinanceManager.Application.Tests.Validators;

public class UpdateTransactionValidatorTests
{
    private readonly UpdateTransactionValidator _validator;

    public UpdateTransactionValidatorTests()
    {
        _validator = new UpdateTransactionValidator();
    }

    [Fact]
    public void ShouldHaveError_WhenSourceAccountIdIsEmpty()
    {
        // Arrange
        var dto = new UpdateTransactionDto
        {
            SourceAccountId = 0, // Invalid
            DestinationAccountId = 1,
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test Description"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateTransactionDto.SourceAccountId));
    }

    [Fact]
    public void ShouldHaveError_WhenDestinationAccountIdIsEmpty()
    {
        // Arrange
        var dto = new UpdateTransactionDto
        {
            SourceAccountId = 1,
            DestinationAccountId = 0, // Invalid
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test Description"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateTransactionDto.DestinationAccountId));
    }

    [Fact]
    public void ShouldHaveError_WhenDateIsEmpty()
    {
        // Arrange
        var dto = new UpdateTransactionDto
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 100,
            Date = default, // Invalid
            Description = "Test Description"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateTransactionDto.Date));
    }

    [Fact]
    public void ShouldHaveError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var dto = new UpdateTransactionDto
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 100,
            Date = DateTime.Now,
            Description = new string('a', 501) // Invalid
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateTransactionDto.Description));
    }

    [Fact]
    public void ShouldNotHaveError_WhenDtoIsValid()
    {
        // Arrange
        var dto = new UpdateTransactionDto
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 100,
            Date = DateTime.Now,
            Description = "Valid Description"
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
