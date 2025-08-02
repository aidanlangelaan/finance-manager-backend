using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Transactions.Dtos;
using FinanceManager.Application.Transactions.Mapping;
using FinanceManager.Application.Transactions.Services;
using FinanceManager.Domain.Entities;
using FinanceManager.TestUtilities.Auth;
using Moq;
using Shouldly;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Application.Common.Exceptions;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.Tests.Services;

public class TransactionServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IAccountRepository> _accountRepositoryMock;
    private readonly Mock<IPagingService> _pagingServiceMock;
    private readonly ICurrentUserService _currentUserService;
    private readonly TransactionMapper _mapper;
    private readonly TransactionService _sut;

    public TransactionServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _unitOfWorkMock.Setup(uow => uow.Transactions).Returns(_transactionRepositoryMock.Object);
        _unitOfWorkMock.Setup(uow => uow.Accounts).Returns(_accountRepositoryMock.Object);
        _pagingServiceMock = new Mock<IPagingService>();
        _currentUserService = new TestCurrentUserService();
        _mapper = new TransactionMapper();
        _sut = new TransactionService(_unitOfWorkMock.Object, _currentUserService, _pagingServiceMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTransactionAndReturnId()
    {
        // Arrange
        var createDto = new CreateTransactionDto
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test Transaction"
        };

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 1, Name = "Test Account 1", Type = AccountType.Asset, CurrentBalance = 0 });
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(2, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 2, Name = "Test Account 2", Type = AccountType.Asset, CurrentBalance = 0 });

        _transactionRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Callback<Transaction, CancellationToken>((transaction, ct) => transaction.Id = 1);

        // Act
        var result = await _sut.CreateAsync(createDto, CancellationToken.None);

        // Assert
        _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.ShouldBe(1);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenSourceAccountNotFound()
    {
        // Arrange
        var createDto = new CreateTransactionDto
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test Transaction"
        };

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Account);

        // Act & Assert
        await Should.ThrowAsync<NotFoundException>(() => _sut.CreateAsync(createDto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenDestinationAccountNotFound()
    {
        // Arrange
        var createDto = new CreateTransactionDto
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test Transaction"
        };

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 1, Name = "Test Account 1", Type = AccountType.Asset, CurrentBalance = 0 });
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(2, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Account);

        // Act & Assert
        await Should.ThrowAsync<NotFoundException>(() => _sut.CreateAsync(createDto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenCategoryNotFound()
    {
        // Arrange
        var createDto = new CreateTransactionDto
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test Transaction",
            CategoryId = 999
        };

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 1, Name = "Test Account 1", Type = AccountType.Asset, CurrentBalance = 0 });
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(2, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 2, Name = "Test Account 2", Type = AccountType.Asset, CurrentBalance = 0 });
        _unitOfWorkMock.Setup(uow => uow.Categories.GetByIdAsync(999, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Category);

        // Act & Assert
        await Should.ThrowAsync<NotFoundException>(() => _sut.CreateAsync(createDto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTransactionWithCategory_WhenCategoryExists()
    {
        // Arrange
        var createDto = new CreateTransactionDto
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test Transaction",
            CategoryId = 1
        };

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 1, Name = "Test Account 1", Type = AccountType.Asset, CurrentBalance = 0 });
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(2, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 2, Name = "Test Account 2", Type = AccountType.Asset, CurrentBalance = 0 });
        _unitOfWorkMock.Setup(uow => uow.Categories.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { Id = 1, Name = "Test Category" });

        _transactionRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
            .Callback<Transaction, CancellationToken>((transaction, ct) => transaction.Id = 1);

        // Act
        var result = await _sut.CreateAsync(createDto, CancellationToken.None);

        // Assert
        _transactionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.ShouldBe(1);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTransaction_WhenTransactionExists()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = 1, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now,
            CreatedById = _currentUserService.UserId
        };
        _transactionRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        // Act
        var result = await _sut.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(1);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTransactionDoesNotExist()
    {
        // Arrange
        _transactionRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Transaction);

        // Act
        var result = await _sut.GetByIdAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPagedTransactions()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now,
                CreatedById = _currentUserService.UserId
            },
            new Transaction
            {
                Id = 2, SourceAccountId = 1, DestinationAccountId = 2, Amount = 200, Date = DateTime.Now,
                CreatedById = _currentUserService.UserId
            }
        };

        var pagedResult = new PagedResult<TransactionResponseDto>
        {
            Items = transactions.Select(_mapper.ToDto).ToList(),
            TotalCount = transactions.Count,
            PageNumber = 1,
            PageSize = 10
        };

        _transactionRepositoryMock.Setup(r => r.GetAllAsync(_currentUserService.UserId)).Returns(transactions.AsQueryable());
        _pagingServiceMock.Setup(p => p.ToPagedResultAsync(It.IsAny<IQueryable<Transaction>>(), It.IsAny<PagedRequest>(),
                It.IsAny<Func<Transaction, TransactionResponseDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _sut.GetAllAsync(new PagedRequest { PageNumber = 1, PageSize = 10 }, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Count().ShouldBe(2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTransaction_WhenTransactionExists()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = 1, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now,
            CreatedById = _currentUserService.UserId
        };
        _transactionRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 1, Name = "Test Account 1", Type = AccountType.Asset, CurrentBalance = 0 });
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(2, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 2, Name = "Test Account 2", Type = AccountType.Asset, CurrentBalance = 0 });

        var updateDto = new UpdateTransactionDto { SourceAccountId = 1, DestinationAccountId = 2, Amount = 200, Date = DateTime.Now };

        // Act
        var result = await _sut.UpdateAsync(1, updateDto, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _transactionRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenTransactionDoesNotExist()
    {
        // Arrange
        _transactionRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Transaction);
        var updateDto = new UpdateTransactionDto { SourceAccountId = 1, DestinationAccountId = 2, Amount = 200, Date = DateTime.Now };

        // Act
        var result = await _sut.UpdateAsync(1, updateDto, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenSourceAccountNotFound()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = 1, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now,
            CreatedById = _currentUserService.UserId
        };
        _transactionRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Account);

        var updateDto = new UpdateTransactionDto { SourceAccountId = 1, DestinationAccountId = 2, Amount = 200, Date = DateTime.Now };

        // Act & Assert
        await Should.ThrowAsync<NotFoundException>(() => _sut.UpdateAsync(1, updateDto, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenDestinationAccountNotFound()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = 1, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now,
            CreatedById = _currentUserService.UserId
        };
        _transactionRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 1, Name = "Test Account 1", Type = AccountType.Asset, CurrentBalance = 0 });
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(2, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Account);

        var updateDto = new UpdateTransactionDto { SourceAccountId = 1, DestinationAccountId = 2, Amount = 200, Date = DateTime.Now };

        // Act & Assert
        await Should.ThrowAsync<NotFoundException>(() => _sut.UpdateAsync(1, updateDto, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFoundException_WhenCategoryNotFound()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = 1, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now,
            CreatedById = _currentUserService.UserId
        };
        _transactionRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 1, Name = "Test Account 1", Type = AccountType.Asset, CurrentBalance = 0 });
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(2, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 2, Name = "Test Account 2", Type = AccountType.Asset, CurrentBalance = 0 });
        _unitOfWorkMock.Setup(uow => uow.Categories.GetByIdAsync(999, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Category);

        var updateDto = new UpdateTransactionDto { SourceAccountId = 1, DestinationAccountId = 2, Amount = 200, Date = DateTime.Now, CategoryId = 999 };

        // Act & Assert
        await Should.ThrowAsync<NotFoundException>(() => _sut.UpdateAsync(1, updateDto, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTransactionWithCategory_WhenCategoryExists()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = 1, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now,
            CreatedById = _currentUserService.UserId
        };
        _transactionRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        _accountRepositoryMock.Setup(r => r.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 1, Name = "Test Account 1", Type = AccountType.Asset, CurrentBalance = 0 });
        _accountRepositoryMock.Setup(r => r.GetByIdAsync(2, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Account { Id = 2, Name = "Test Account 2", Type = AccountType.Asset, CurrentBalance = 0 });
        _unitOfWorkMock.Setup(uow => uow.Categories.GetByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { Id = 1, Name = "Test Category" });

        var updateDto = new UpdateTransactionDto { SourceAccountId = 1, DestinationAccountId = 2, Amount = 200, Date = DateTime.Now, CategoryId = 1 };

        // Act
        var result = await _sut.UpdateAsync(1, updateDto, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _transactionRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTransaction_WhenTransactionExists()
    {
        // Arrange
        var transaction = new Transaction
        {
            Id = 1, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now,
            CreatedById = _currentUserService.UserId
        };
        _transactionRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(transaction);

        // Act
        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _transactionRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenTransactionDoesNotExist()
    {
        // Arrange
        _transactionRepositoryMock.Setup(r =>
                r.FindEntityByIdAsync(1, _currentUserService.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Transaction);

        // Act
        var result = await _sut.DeleteAsync(1, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }
}