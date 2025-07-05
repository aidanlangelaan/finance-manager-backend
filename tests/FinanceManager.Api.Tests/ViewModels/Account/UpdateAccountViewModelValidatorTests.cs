using FinanceManager.Api.ViewModels.Account;
using FinanceManager.Api.ViewModels.Account.Validators;
using FinanceManager.Domain.Enums;
using Shouldly;

namespace FinanceManager.Api.Tests.ViewModels.Account;

public class UpdateAccountViewModelValidatorTests
{
    private readonly UpdateAccountViewModelValidator _validator = new();

    [Fact]
    public void ShouldHaveError_WhenNameIsEmpty()
    {
        // Arrange
        var viewModel = new UpdateAccountViewModel { Name = string.Empty, Type = AccountType.Asset };

        // Act
        var result = _validator.Validate(viewModel);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateAccountViewModel.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenNameIsTooLong()
    {
        // Arrange
        var viewModel = new UpdateAccountViewModel { Name = new string('a', 256), Type = AccountType.Asset };

        // Act
        var result = _validator.Validate(viewModel);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateAccountViewModel.Name));
    }

    [Fact]
    public void ShouldHaveError_WhenTypeIsInvalid()
    {
        // Arrange
        var viewModel = new UpdateAccountViewModel { Name = "Test Account", Type = (AccountType)99 };

        // Act
        var result = _validator.Validate(viewModel);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateAccountViewModel.Type));
    }

    [Fact]
    public void ShouldHaveError_WhenDescriptionIsTooLong()
    {
        // Arrange
        var viewModel = new UpdateAccountViewModel { Name = "Test Account", Type = AccountType.Asset, Description = new string('a', 256) };

        // Act
        var result = _validator.Validate(viewModel);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == nameof(UpdateAccountViewModel.Description));
    }

    [Fact]
    public void ShouldNotHaveError_WhenViewModelIsValid()
    {
        // Arrange
        var viewModel = new UpdateAccountViewModel { Name = "Test Account", Type = AccountType.Asset, Description = "Some description" };

        // Act
        var result = _validator.Validate(viewModel);

        // Assert
        result.IsValid.ShouldBeTrue();
    }
}
