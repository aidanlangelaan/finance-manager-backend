using FinanceManager.Domain.Exceptions;
using Shouldly;
using Xunit;

namespace FinanceManager.Domain.Tests.Exceptions;

public class MissingClaimExceptionTests
{
    [Fact]
    public void Constructor_ShouldCreateExceptionWithDefaultMessage()
    {
        // Arrange & Act
        var exception = new MissingClaimException("Test message");

        // Assert
        exception.Message.ShouldBe("Test message");
    }
}