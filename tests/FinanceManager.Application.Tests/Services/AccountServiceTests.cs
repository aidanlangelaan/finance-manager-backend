using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Accounts.Mapping;
using FinanceManager.Application.Accounts.Services;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Domain.Entities;
using FinanceManager.TestUtilities.Auth;
using Moq;
using Shouldly;

namespace FinanceManager.Application.Tests.Services;

public class AccountServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IPagingService> _pagingServiceMock;
    private readonly ICurrentUserService _currentUserService;
    private readonly AccountMapper _mapper;
    private readonly AccountService _sut;

    public AccountServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _unitOfWorkMock.Setup(uow => uow.Accounts).Returns(_accountRepositoryMock.Object);
        _pagingServiceMock = new Mock<IPagingService>();
        _currentUserService = new TestCurrentUserService();
        _mapper = new AccountMapper();
        _sut = new AccountService(_unitOfWorkMock.Object, _currentUserService, _pagingServiceMock.Object, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAccount_WhenAccountExists()
    {
        // Arrange
        var account = new Account
        {
            Id = 1, Name = "Test Account", Type = Domain.Enums.AccountType.Asset, CurrentBalance = 1000,
            CreatedById = _currentUserService.UserId
        };
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        var result = await _sut.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenAccountDoesNotExist()
    {
        // Arrange
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Account);

        // Act
        var result = await _sut.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedAccounts()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new Account
            {
                Id = 1, Name = "Test Account 1", Type = Domain.Enums.AccountType.Asset, CurrentBalance = 1000,
                CreatedById = _currentUserService.UserId
            },
            new Account
            {
                Id = 2, Name = "Test Account 2", Type = Domain.Enums.AccountType.Asset, CurrentBalance = 2000,
                CreatedById = _currentUserService.UserId
            }
        };

        var pagedResult = new PagedResult<AccountResponseDto>
        {
            Items = accounts.Select(_mapper.ToDto).ToList(),
            TotalCount = accounts.Count,
            PageNumber = 1,
            PageSize = 10
        };

        _accountRepositoryMock.Setup(r => r.GetAllAsync(_currentUserService.UserId)).Returns(accounts.AsQueryable());
        _pagingServiceMock.Setup(p => p.ToPagedResultAsync(It.IsAny<IQueryable<Account>>(), It.IsAny<PagedRequest>(),
                It.IsAny<Func<Account, AccountResponseDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _sut.GetAllAsync(new PagedRequest { PageNumber = 1, PageSize = 10 }, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count().ShouldBe(2);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAccountAndReturnId()
    {
        // Arrange
        var createDto = new CreateAccountDto
            { Name = "Test Account", Type = Domain.Enums.AccountType.Asset, CurrentBalance = 1000 };

        _accountRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
            .Callback<Account, CancellationToken>((account, ct) => account.Id = 1);

        // Act
        var result = await _sut.CreateAsync(createDto, CancellationToken.None);

        // Assert
        _accountRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.ShouldBe(1);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAccount_WhenAccountExists()
    {
        // Arrange
        var account = new Account
        {
            Id = 1, Name = "Test Account", Type = Domain.Enums.AccountType.Asset, CurrentBalance = 1000,
            CreatedById = _currentUserService.UserId
        };
        _accountRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);
        var updateDto = new UpdateAccountDto { Name = "Updated Account" };

        // Act
        var result = await _sut.UpdateAsync(1, updateDto, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _accountRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenAccountDoesNotExist()
    {
        // Arrange
        _accountRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Account);
        var updateDto = new UpdateAccountDto { Name = "Updated Account" };

        // Act
        var result = await _sut.UpdateAsync(1, updateDto, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAccount_WhenAccountExists()
    {
        // Arrange
        var account = new Account
        {
            Id = 1, Name = "Test Account", Type = Domain.Enums.AccountType.Asset, CurrentBalance = 1000,
            CreatedById = _currentUserService.UserId
        };
        _accountRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(account);

        // Act
        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _accountRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenAccountDoesNotExist()
    {
        // Arrange
        _accountRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Account);

        // Act
        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }
}
