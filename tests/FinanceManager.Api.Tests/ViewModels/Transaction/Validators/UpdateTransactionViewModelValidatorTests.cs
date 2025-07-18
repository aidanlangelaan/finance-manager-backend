using FinanceManager.Api.ViewModels.Transaction;
using FinanceManager.Api.ViewModels.Transaction.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace FinanceManager.Api.Tests.ViewModels.Transaction.Validators;

public class UpdateTransactionViewModelValidatorTests
{
    private readonly UpdateTransactionViewModelValidator _validator;

    public UpdateTransactionViewModelValidatorTests()
    {
        _validator = new UpdateTransactionViewModelValidator();
    }

    [Fact]
    public void Should_have_error_when_SourceAccountId_is_empty()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 0, DestinationAccountId = 1, Amount = 100, Date = DateTime.Now };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.SourceAccountId);
    }

    [Fact]
    public void Should_have_error_when_SourceAccountId_is_not_greater_than_zero()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = -1, DestinationAccountId = 1, Amount = 100, Date = DateTime.Now };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.SourceAccountId);
    }

    [Fact]
    public void Should_not_have_error_when_SourceAccountId_is_valid()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 1, Amount = 100, Date = DateTime.Now };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.SourceAccountId);
    }

    [Fact]
    public void Should_have_error_when_DestinationAccountId_is_empty()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 0, Amount = 100, Date = DateTime.Now };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DestinationAccountId);
    }

    [Fact]
    public void Should_have_error_when_DestinationAccountId_is_not_greater_than_zero()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = -1, Amount = 100, Date = DateTime.Now };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DestinationAccountId);
    }

    [Fact]
    public void Should_not_have_error_when_DestinationAccountId_is_valid()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 1, Amount = 100, Date = DateTime.Now };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.DestinationAccountId);
    }

    [Fact]
    public void Should_have_error_when_Amount_is_empty()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 1, Amount = 0, Date = DateTime.Now };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Should_not_have_error_when_Amount_is_valid()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 1, Amount = 100, Date = DateTime.Now };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Should_have_error_when_Date_is_empty()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 1, Amount = 100, Date = default };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void Should_not_have_error_when_Date_is_valid()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 1, Amount = 100, Date = DateTime.Now };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Date);
    }

    [Fact]
    public void Should_have_error_when_Description_is_empty()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 1, Amount = 100, Date = DateTime.Now, Description = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_have_error_when_Description_exceeds_max_length()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 1, Amount = 100, Date = DateTime.Now, Description = new string('a', 501) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_not_have_error_when_Description_is_valid()
    {
        var model = new UpdateTransactionViewModel { SourceAccountId = 1, DestinationAccountId = 1, Amount = 100, Date = DateTime.Now, Description = "Valid description" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}