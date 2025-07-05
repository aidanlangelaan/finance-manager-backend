using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;
using FinanceManager.Persistence.Repositories;
using FinanceManager.Persistence.Tests.Common;
using Shouldly;

namespace FinanceManager.Persistence.Tests.Repositories;

public class AccountRepositoryTests : PersistenceTestBase
{
    private AccountRepository _sut;

    public AccountRepositoryTests()
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _sut = new AccountRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddAccountToDatabase()
    {
        // Arrange
        var account = new Account
        {
            Name = "Test Account",
            Type = AccountType.Asset,
            CurrentBalance = 100
        };

        // Act
        await _sut.AddAsync(account, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var retrievedAccount = await _sut.GetByIdAsync(account.Id, 1, CancellationToken.None);
        retrievedAccount.ShouldNotBeNull();
        retrievedAccount.Name.ShouldBe(account.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAccount_WhenAccountExistsAndBelongsToUser()
    {
        // Arrange
        var userId = 1;
        var account = new Account
        {
            Name = "Test Account",
            Type = AccountType.Asset,
            CurrentBalance = 100
        };
        await _sut.AddAsync(account, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Act
        var retrievedAccount = await _sut.GetByIdAsync(account.Id, userId, CancellationToken.None);

        // Assert
        retrievedAccount.ShouldNotBeNull();
        retrievedAccount.Id.ShouldBe(account.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenAccountDoesNotExist()
    {
        // Arrange
        var userId = 1;

        // Act
        var retrievedAccount = await _sut.GetByIdAsync(999, userId, CancellationToken.None);

        // Assert
        retrievedAccount.ShouldBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenAccountDoesNotBelongToUser()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(otherUserId);
        var account = new Account
        {
            Name = "Test Account",
            Type = AccountType.Asset,
            CurrentBalance = 100
        };
        await _sut.AddAsync(account, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        // Act
        var retrievedAccount = await _sut.GetByIdAsync(account.Id, userId, CancellationToken.None);

        // Assert
        retrievedAccount.ShouldBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAccountsForUser()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        await _sut.AddAsync(new Account { Name = "User Account 1", Type = AccountType.Asset, CurrentBalance = 100 }, CancellationToken.None);
        await _sut.AddAsync(new Account { Name = "User Account 2", Type = AccountType.Asset, CurrentBalance = 200 }, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(otherUserId);
        await _sut.AddAsync(new Account { Name = "Other User Account", Type = AccountType.Asset, CurrentBalance = 300 }, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        // Act
        var accounts = _sut.GetAllAsync(userId).ToList();

        // Assert
        accounts.Count.ShouldBe(2);
        accounts.ShouldContain(a => a.Name == "User Account 1");
        accounts.ShouldContain(a => a.Name == "User Account 2");
        accounts.ShouldNotContain(a => a.Name == "Other User Account");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAccountInDatabase()
    {
        // Arrange
        var userId = 1;
        var account = new Account
        {
            Name = "Original Name",
            Type = AccountType.Asset,
            CurrentBalance = 100
        };
        await _sut.AddAsync(account, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        account.Name = "Updated Name";
        account.CurrentBalance = 200;

        // Act
        await _sut.UpdateAsync(account, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var retrievedAccount = await _sut.GetByIdAsync(account.Id, userId, CancellationToken.None);
        retrievedAccount.ShouldNotBeNull();
        retrievedAccount.Name.ShouldBe("Updated Name");
        retrievedAccount.CurrentBalance.ShouldBe(200);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAccountFromDatabase()
    {
        // Arrange
        var userId = 1;
        var account = new Account
        {
            Name = "Account to Delete",
            Type = AccountType.Asset,
            CurrentBalance = 100
        };
        await _sut.AddAsync(account, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(account, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var retrievedAccount = await _sut.GetByIdAsync(account.Id, userId, CancellationToken.None);
        retrievedAccount.ShouldBeNull();
    }

    [Fact]
    public async Task FindEntityByIdAsync_ShouldReturnAccount_WhenAccountExistsAndBelongsToUser()
    {
        // Arrange
        var userId = 1;
        var account = new Account
        {
            Name = "Test Account",
            Type = AccountType.Asset,
            CurrentBalance = 100
        };
        await _sut.AddAsync(account, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Act
        var retrievedAccount = await _sut.FindEntityByIdAsync(account.Id, userId, CancellationToken.None);

        // Assert
        retrievedAccount.ShouldNotBeNull();
        retrievedAccount.Id.ShouldBe(account.Id);
    }

    [Fact]
    public async Task FindEntityByIdAsync_ShouldReturnNull_WhenAccountDoesNotExist()
    {
        // Arrange
        var userId = 1;

        // Act
        var retrievedAccount = await _sut.FindEntityByIdAsync(999, userId, CancellationToken.None);

        // Assert
        retrievedAccount.ShouldBeNull();
    }

    [Fact]
    public async Task FindEntityByIdAsync_ShouldReturnNull_WhenAccountDoesNotBelongToUser()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(otherUserId);
        var account = new Account
        {
            Name = "Test Account",
            Type = AccountType.Asset,
            CurrentBalance = 100
        };
        await _sut.AddAsync(account, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        // Act
        var retrievedAccount = await _sut.FindEntityByIdAsync(account.Id, userId, CancellationToken.None);

        // Assert
        retrievedAccount.ShouldBeNull();
    }
}
