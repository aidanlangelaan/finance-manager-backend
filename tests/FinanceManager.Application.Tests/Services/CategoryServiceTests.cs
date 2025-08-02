using FinanceManager.Application.Categories.Dtos;
using FinanceManager.Application.Categories.Mapping;
using FinanceManager.Application.Categories.Services;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Domain.Entities;
using FinanceManager.TestUtilities.Auth;
using Moq;
using Shouldly;

namespace FinanceManager.Application.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IPagingService> _pagingServiceMock;
    private readonly ICurrentUserService _currentUserService;
    private readonly CategoryMapper _mapper;
    private readonly CategoryService _sut;

    public CategoryServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock.Setup(uow => uow.Categories).Returns(_categoryRepositoryMock.Object);
        _pagingServiceMock = new Mock<IPagingService>();
        _currentUserService = new TestCurrentUserService();
        _mapper = new CategoryMapper();
        _sut = new CategoryService(_unitOfWorkMock.Object, _currentUserService, _pagingServiceMock.Object, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category
        {
            Id = 1, Name = "Test Category",
            CreatedById = _currentUserService.UserId
        };
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        // Act
        var result = await _sut.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Category);

        // Act
        var result = await _sut.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category
            {
                Id = 1, Name = "Test Category 1",
                CreatedById = _currentUserService.UserId
            },
            new Category
            {
                Id = 2, Name = "Test Category 2",
                CreatedById = _currentUserService.UserId
            }
        };

        var pagedResult = new PagedResult<CategoryResponseDto>
        {
            Items = categories.Select(_mapper.ToDto).ToList(),
            TotalCount = categories.Count,
            PageNumber = 1,
            PageSize = 10
        };

        _categoryRepositoryMock.Setup(r => r.GetAllAsync(_currentUserService.UserId)).Returns(categories.AsQueryable());
        _pagingServiceMock.Setup(p => p.ToPagedResultAsync(It.IsAny<IQueryable<Category>>(), It.IsAny<PagedRequest>(),
                It.IsAny<Func<Category, CategoryResponseDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _sut.GetAllAsync(new PagedRequest { PageNumber = 1, PageSize = 10 }, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count().ShouldBe(2);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCategoryAndReturnId()
    {
        // Arrange
        var createDto = new CreateCategoryDto
            { Name = "Test Category" };

        _categoryRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((category, ct) => category.Id = 1);

        // Act
        var result = await _sut.CreateAsync(createDto, CancellationToken.None);

        // Assert
        _categoryRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.ShouldBe(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category
        {
            Id = 1, Name = "Test Category",
            CreatedById = _currentUserService.UserId
        };
        _categoryRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        var updateDto = new UpdateCategoryDto { Name = "Updated Category" };

        // Act
        var result = await _sut.UpdateAsync(1, updateDto, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _categoryRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenCategoryDoesNotExist()
    {
        // Arrange
        _categoryRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Category);
        var updateDto = new UpdateCategoryDto { Name = "Updated Category" };

        // Act
        var result = await _sut.UpdateAsync(1, updateDto, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteCategory_WhenCategoryExists()
    {
        // Arrange
        var category = new Category
        {
            Id = 1, Name = "Test Category",
            CreatedById = _currentUserService.UserId
        };
        _categoryRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        // Act
        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _categoryRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenCategoryDoesNotExist()
    {
        // Arrange
        _categoryRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Category);

        // Act
        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }
}
