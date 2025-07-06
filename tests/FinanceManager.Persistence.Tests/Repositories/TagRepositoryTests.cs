using FinanceManager.Domain.Entities;
using FinanceManager.Persistence.Repositories;
using FinanceManager.Persistence.Tests.Common;
using Shouldly;

namespace FinanceManager.Persistence.Tests.Repositories;

public class TagRepositoryTests : PersistenceTestBase
{
    private TagRepository _sut = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _sut = new TagRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTagToDatabase()
    {
        // Arrange
        var tag = new Tag
        {
            Name = "Test Tag"
        };

        // Act
        await _sut.AddAsync(tag, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var retrievedTag = await _sut.GetByIdAsync(tag.Id, 1, CancellationToken.None);
        retrievedTag.ShouldNotBeNull();
        retrievedTag.Name.ShouldBe(tag.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTag_WhenTagExistsAndBelongsToUser()
    {
        // Arrange
        var userId = 1;
        var tag = new Tag
        {
            Name = "Test Tag"
        };
        await _sut.AddAsync(tag, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Act
        var retrievedTag = await _sut.GetByIdAsync(tag.Id, userId, CancellationToken.None);

        // Assert
        retrievedTag.ShouldNotBeNull();
        retrievedTag.Id.ShouldBe(tag.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTagDoesNotExist()
    {
        // Arrange
        var userId = 1;

        // Act
        var retrievedTag = await _sut.GetByIdAsync(999, userId, CancellationToken.None);

        // Assert
        retrievedTag.ShouldBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTagDoesNotBelongToUser()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(otherUserId);
        var tag = new Tag
        {
            Name = "Test Tag"
        };
        await _sut.AddAsync(tag, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        // Act
        var retrievedTag = await _sut.GetByIdAsync(tag.Id, userId, CancellationToken.None);

        // Assert
        retrievedTag.ShouldBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTagsForUser()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        await _sut.AddAsync(new Tag { Name = "User Tag 1" }, CancellationToken.None);
        await _sut.AddAsync(new Tag { Name = "User Tag 2" }, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(otherUserId);
        await _sut.AddAsync(new Tag { Name = "Other User Tag" }, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        // Act
        var tags = _sut.GetAllAsync(userId).ToList();

        // Assert
        tags.Count.ShouldBe(2);
        tags.ShouldContain(a => a.Name == "User Tag 1");
        tags.ShouldContain(a => a.Name == "User Tag 2");
        tags.ShouldNotContain(a => a.Name == "Other User Tag");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTagInDatabase()
    {
        // Arrange
        var userId = 1;
        var tag = new Tag
        {
            Name = "Original Name"
        };
        await _sut.AddAsync(tag, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        tag.Name = "Updated Name";

        // Act
        await _sut.UpdateAsync(tag, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var retrievedTag = await _sut.GetByIdAsync(tag.Id, userId, CancellationToken.None);
        retrievedTag.ShouldNotBeNull();
        retrievedTag.Name.ShouldBe("Updated Name");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTagFromDatabase()
    {
        // Arrange
        var userId = 1;
        var tag = new Tag
        {
            Name = "Tag to Delete"
        };
        await _sut.AddAsync(tag, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(tag, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var retrievedTag = await _sut.GetByIdAsync(tag.Id, userId, CancellationToken.None);
        retrievedTag.ShouldBeNull();
    }

    [Fact]
    public async Task FindEntityByIdAsync_ShouldReturnTag_WhenTagExistsAndBelongsToUser()
    {
        // Arrange
        var userId = 1;
        var tag = new Tag
        {
            Name = "Test Tag"
        };
        await _sut.AddAsync(tag, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Act
        var retrievedTag = await _sut.FindEntityByIdAsync(tag.Id, userId, CancellationToken.None);

        // Assert
        retrievedTag.ShouldNotBeNull();
        retrievedTag.Id.ShouldBe(tag.Id);
    }

    [Fact]
    public async Task FindEntityByIdAsync_ShouldReturnNull_WhenTagDoesNotExist()
    {
        // Arrange
        var userId = 1;

        // Act
        var retrievedTag = await _sut.FindEntityByIdAsync(999, userId, CancellationToken.None);

        // Assert
        retrievedTag.ShouldBeNull();
    }

    [Fact]
    public async Task FindEntityByIdAsync_ShouldReturnNull_WhenTagDoesNotBelongToUser()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(otherUserId);
        var tag = new Tag
        {
            Name = "Test Tag"
        };
        await _sut.AddAsync(tag, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        // Act
        var retrievedTag = await _sut.FindEntityByIdAsync(tag.Id, userId, CancellationToken.None);

        // Assert
        retrievedTag.ShouldBeNull();
    }
}
