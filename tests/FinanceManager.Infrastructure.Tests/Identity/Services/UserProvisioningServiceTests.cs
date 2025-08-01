using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using FinanceManager.Infrastructure.Identity.Services;
using Moq;
using Shouldly;

namespace FinanceManager.Infrastructure.Tests.Identity.Services;

public class UserProvisioningServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserProvisioningServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldCreateUser_WhenUserDoesNotExist()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var name = "Test User";
        var email = "test@example.com";

        _userRepositoryMock.Setup(r => r.GetUserByKeycloakIdAsync(keycloakId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        var sut = new UserProvisioningService(_unitOfWorkMock.Object);

        // Act
        var result = await sut.GetOrCreateUserAsync(keycloakId, name, email);

        // Assert
        result.ShouldNotBeNull();
        result.KeycloakId.ShouldBe(keycloakId);
        result.DisplayName.ShouldBe(name);
        result.Email.ShouldBe(email);
        _userRepositoryMock.Verify(r => r.AddUserAsync(It.Is<User>(u => u.KeycloakId == keycloakId && u.DisplayName == name && u.Email == email), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldReturnExistingUser_WhenUserExists()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var existingUser = new User { Id = 1, KeycloakId = keycloakId, DisplayName = "Existing User", Email = "existing@example.com" };

        _userRepositoryMock.Setup(r => r.GetUserByKeycloakIdAsync(keycloakId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var sut = new UserProvisioningService(_unitOfWorkMock.Object);

        // Act
        var result = await sut.GetOrCreateUserAsync(keycloakId, "New Name", "new@example.com");

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(existingUser.Id);
        result.DisplayName.ShouldBe("New Name");
        result.Email.ShouldBe("new@example.com");
        _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldUpdateUser_WhenUserDataChanges()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var existingUser = new User { Id = 1, KeycloakId = keycloakId, DisplayName = "Old Name", Email = "old@example.com" };

        _userRepositoryMock.Setup(r => r.GetUserByKeycloakIdAsync(keycloakId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var sut = new UserProvisioningService(_unitOfWorkMock.Object);

        // Act
        var result = await sut.GetOrCreateUserAsync(keycloakId, "New Name", "new@example.com");

        // Assert
        result.ShouldNotBeNull();
        result.DisplayName.ShouldBe("New Name");
        result.Email.ShouldBe("new@example.com");
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldThrowException_WhenNameIsMissingForNewUser()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var email = "test@example.com";

        _userRepositoryMock.Setup(r => r.GetUserByKeycloakIdAsync(keycloakId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        var sut = new UserProvisioningService(_unitOfWorkMock.Object);

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(() => sut.GetOrCreateUserAsync(keycloakId, null, email));
        _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldThrowException_WhenEmailIsMissingForNewUser()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var name = "Test User";

        _userRepositoryMock.Setup(r => r.GetUserByKeycloakIdAsync(keycloakId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null!);

        var sut = new UserProvisioningService(_unitOfWorkMock.Object);

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(() => sut.GetOrCreateUserAsync(keycloakId, name, null));
        _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
