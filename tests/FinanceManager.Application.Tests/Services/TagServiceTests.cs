using FinanceManager.Application.Tags.Dtos;
using FinanceManager.Application.Tags.Mapping;
using FinanceManager.Application.Tags.Services;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Common.Interfaces.Paging;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Domain.Entities;
using FinanceManager.TestUtilities.Auth;
using Moq;
using Shouldly;

namespace FinanceManager.Application.Tests.Services;

public class TagServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IPagingService> _pagingServiceMock;
    private readonly ICurrentUserService _currentUserService;
    private readonly TagMapper _mapper;
    private readonly TagService _sut;

    public TagServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _unitOfWorkMock.Setup(uow => uow.Tags).Returns(_tagRepositoryMock.Object);
        _pagingServiceMock = new Mock<IPagingService>();
        _currentUserService = new TestCurrentUserService();
        _mapper = new TagMapper();
        _sut = new TagService(_unitOfWorkMock.Object, _currentUserService, _pagingServiceMock.Object, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTag_WhenTagExists()
    {
        // Arrange
        var tag = new Tag
        {
            Id = 1, Name = "Test Tag",
            CreatedById = _currentUserService.UserId
        };
        _tagRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        var result = await _sut.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTagDoesNotExist()
    {
        // Arrange
        _tagRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Tag);

        // Act
        var result = await _sut.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedTags()
    {
        // Arrange
        var tags = new List<Tag>
        {
            new Tag
            {
                Id = 1, Name = "Test Tag 1",
                CreatedById = _currentUserService.UserId
            },
            new Tag
            {
                Id = 2, Name = "Test Tag 2",
                CreatedById = _currentUserService.UserId
            }
        };

        var pagedResult = new PagedResult<TagResponseDto>
        {
            Items = tags.Select(_mapper.ToDto).ToList(),
            TotalCount = tags.Count,
            PageNumber = 1,
            PageSize = 10
        };

        _tagRepositoryMock.Setup(r => r.GetAllAsync(_currentUserService.UserId)).Returns(tags.AsQueryable());
        _pagingServiceMock.Setup(p => p.ToPagedResultAsync(It.IsAny<IQueryable<Tag>>(), It.IsAny<PagedRequest>(),
                It.IsAny<Func<Tag, TagResponseDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _sut.GetAllAsync(new PagedRequest { PageNumber = 1, PageSize = 10 }, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count().ShouldBe(2);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTagAndReturnId()
    {
        // Arrange
        var createDto = new CreateTagDto
            { Name = "Test Tag" };

        _tagRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()))
            .Callback<Tag, CancellationToken>((tag, ct) => tag.Id = 1);

        // Act
        var result = await _sut.CreateAsync(createDto, CancellationToken.None);

        // Assert
        _tagRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.ShouldBe(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTag_WhenTagExists()
    {
        // Arrange
        var tag = new Tag
        {
            Id = 1, Name = "Test Tag",
            CreatedById = _currentUserService.UserId
        };
        _tagRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);
        var updateDto = new UpdateTagDto { Name = "Updated Tag" };

        // Act
        var result = await _sut.UpdateAsync(1, updateDto, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _tagRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenTagDoesNotExist()
    {
        // Arrange
        _tagRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Tag);
        var updateDto = new UpdateTagDto { Name = "Updated Tag" };

        // Act
        var result = await _sut.UpdateAsync(1, updateDto, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTag_WhenTagExists()
    {
        // Arrange
        var tag = new Tag
        {
            Id = 1, Name = "Test Tag",
            CreatedById = _currentUserService.UserId
        };
        _tagRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(tag);

        // Act
        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _tagRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Tag>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenTagDoesNotExist()
    {
        // Arrange
        _tagRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Tag);

        // Act
        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }
}
