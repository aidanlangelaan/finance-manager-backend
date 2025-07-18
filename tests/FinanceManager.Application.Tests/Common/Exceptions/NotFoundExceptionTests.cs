using FinanceManager.Application.Common.Exceptions;
using Shouldly;

namespace FinanceManager.Application.Tests.Common.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void Constructor_ShouldCreateExceptionWithDefaultMessage()
    {
        // Arrange & Act
        var exception = new NotFoundException();

        // Assert
        exception.Message.ShouldBe("Exception of type 'FinanceManager.Application.Common.Exceptions.NotFoundException' was thrown.");
    }

    [Fact]
    public void Constructor_WithMessage_ShouldCreateExceptionWithProvidedMessage()
    {
        // Arrange
        var message = "Test message";

        // Act
        var exception = new NotFoundException(message);

        // Assert
        exception.Message.ShouldBe(message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldCreateExceptionWithProvidedMessageAndInnerException()
    {
        // Arrange
        var message = "Test message";
        var innerException = new Exception("Inner exception");

        // Act
        var exception = new NotFoundException(message, innerException);

        // Assert
        exception.Message.ShouldBe(message);
        exception.InnerException.ShouldBe(innerException);
    }

    [Fact]
    public void Constructor_WithNameAndKey_ShouldCreateExceptionWithFormattedMessage()
    {
        // Arrange
        var name = "TestEntity";
        var key = 123;

        // Act
        var exception = new NotFoundException(name, key);

        // Assert
        exception.Message.ShouldBe($"Entity \"{name}\" ({key}) was not found.");
    }
}


