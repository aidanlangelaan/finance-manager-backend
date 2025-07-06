using FinanceManager.Domain.Entities;
using FinanceManager.Persistence.Repositories;
using FinanceManager.Persistence.Tests.Common;
using Shouldly;

namespace FinanceManager.Persistence.Tests.Repositories;

public class CategoryRepositoryTests : PersistenceTestBase
{
    private CategoryRepository _sut = null!;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        _sut = new CategoryRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCategoryToDatabase()
    {
        // Arrange
        var category = new Category
        {
            Name = "Test Category"
        };

        // Act
        await _sut.AddAsync(category, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var retrievedCategory = await _sut.GetByIdAsync(category.Id, 1, CancellationToken.None);
        retrievedCategory.ShouldNotBeNull();
        retrievedCategory.Name.ShouldBe(category.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExistsAndBelongsToUser()
    {
        // Arrange
        var userId = 1;
        var category = new Category
        {
            Name = "Test Category"
        };
        await _sut.AddAsync(category, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Act
        var retrievedCategory = await _sut.GetByIdAsync(category.Id, userId, CancellationToken.None);

        // Assert
        retrievedCategory.ShouldNotBeNull();
        retrievedCategory.Id.ShouldBe(category.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        var userId = 1;

        // Act
        var retrievedCategory = await _sut.GetByIdAsync(999, userId, CancellationToken.None);

        // Assert
        retrievedCategory.ShouldBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryDoesNotBelongToUser()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(otherUserId);
        var category = new Category
        {
            Name = "Test Category"
        };
        await _sut.AddAsync(category, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        // Act
        var retrievedCategory = await _sut.GetByIdAsync(category.Id, userId, CancellationToken.None);

        // Assert
        retrievedCategory.ShouldBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategoriesForUser()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        await _sut.AddAsync(new Category { Name = "User Category 1" }, CancellationToken.None);
        await _sut.AddAsync(new Category { Name = "User Category 2" }, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(otherUserId);
        await _sut.AddAsync(new Category { Name = "Other User Category" }, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        // Act
        var categories = _sut.GetAllAsync(userId).ToList();

        // Assert
        categories.Count.ShouldBe(2);
        categories.ShouldContain(a => a.Name == "User Category 1");
        categories.ShouldContain(a => a.Name == "User Category 2");
        categories.ShouldNotContain(a => a.Name == "Other User Category");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategoryInDatabase()
    {
        // Arrange
        var userId = 1;
        var category = new Category
        {
            Name = "Original Name"
        };
        await _sut.AddAsync(category, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        category.Name = "Updated Name";

        // Act
        await _sut.UpdateAsync(category, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var retrievedCategory = await _sut.GetByIdAsync(category.Id, userId, CancellationToken.None);
        retrievedCategory.ShouldNotBeNull();
        retrievedCategory.Name.ShouldBe("Updated Name");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCategoryFromDatabase()
    {
        // Arrange
        var userId = 1;
        var category = new Category
        {
            Name = "Category to Delete"
        };
        await _sut.AddAsync(category, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(category, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Assert
        var retrievedCategory = await _sut.GetByIdAsync(category.Id, userId, CancellationToken.None);
        retrievedCategory.ShouldBeNull();
    }

    [Fact]
    public async Task FindEntityByIdAsync_ShouldReturnCategory_WhenCategoryExistsAndBelongsToUser()
    {
        // Arrange
        var userId = 1;
        var category = new Category
        {
            Name = "Test Category"
        };
        await _sut.AddAsync(category, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        // Act
        var retrievedCategory = await _sut.FindEntityByIdAsync(category.Id, userId, CancellationToken.None);

        // Assert
        retrievedCategory.ShouldNotBeNull();
        retrievedCategory.Id.ShouldBe(category.Id);
    }

    [Fact]
    public async Task FindEntityByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        var userId = 1;

        // Act
        var retrievedCategory = await _sut.FindEntityByIdAsync(999, userId, CancellationToken.None);

        // Assert
        retrievedCategory.ShouldBeNull();
    }

    [Fact]
    public async Task FindEntityByIdAsync_ShouldReturnNull_WhenCategoryDoesNotBelongToUser()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(otherUserId);
        var category = new Category
        {
            Name = "Test Category"
        };
        await _sut.AddAsync(category, CancellationToken.None);
        await DbContext.SaveChangesAsync();

        CurrentUserServiceMock.Setup(x => x.UserId).Returns(userId);
        // Act
        var retrievedCategory = await _sut.FindEntityByIdAsync(category.Id, userId, CancellationToken.None);

        // Assert
        retrievedCategory.ShouldBeNull();
    }
}
