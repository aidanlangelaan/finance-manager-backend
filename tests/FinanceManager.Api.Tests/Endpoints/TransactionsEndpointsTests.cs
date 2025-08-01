using System.Net;
using System.Text;
using System.Text.Json;
using FinanceManager.Api.Tests.Common;
using FinanceManager.Api.ViewModels.Transaction;
using FinanceManager.Application.Transactions.Dtos;
using FinanceManager.Application.Common.Models.Paging;
using Shouldly;
using System.Net.Http.Json;
using Moq;

namespace FinanceManager.Api.Tests.Endpoints;

public class TransactionsEndpointsTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task CreateTransaction_ShouldReturnCreatedTransaction_WhenValidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var createTransactionViewModel = new CreateTransactionViewModel
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test Transaction"
        };
        var content = new StringContent(JsonSerializer.Serialize(createTransactionViewModel), Encoding.UTF8, "application/json");

        var createdTransactionId = 1;
        factory.TransactionServiceMock.Setup(s => s.CreateAsync(It.IsAny<CreateTransactionDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdTransactionId);
        factory.TransactionServiceMock.Setup(s => s.GetByIdAsync(createdTransactionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TransactionResponseDto
            {
                Id = createdTransactionId,
                SourceAccountId = createTransactionViewModel.SourceAccountId,
                DestinationAccountId = createTransactionViewModel.DestinationAccountId,
                Amount = createTransactionViewModel.Amount,
                Date = createTransactionViewModel.Date,
                Description = createTransactionViewModel.Description
            });

        // Act
        var response = await client.PostAsync("/api/transactions", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var transactionId = await response.Content.ReadFromJsonAsync<int>();
        transactionId.ShouldBe(createdTransactionId);

        var getResponse = await client.GetAsync($"/api/transactions/{transactionId}");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var transactionResponse = await getResponse.Content.ReadFromJsonAsync<TransactionResponseDto>();
        transactionResponse.ShouldNotBeNull();
        transactionResponse.SourceAccountId.ShouldBe(createTransactionViewModel.SourceAccountId);
        transactionResponse.DestinationAccountId.ShouldBe(createTransactionViewModel.DestinationAccountId);
        transactionResponse.Amount.ShouldBe(createTransactionViewModel.Amount);
        transactionResponse.Date.ShouldBe(createTransactionViewModel.Date);
        transactionResponse.Description.ShouldBe(createTransactionViewModel.Description);
    }

    [Fact]
    public async Task CreateTransaction_ShouldReturnBadRequest_WhenInvalidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var createTransactionViewModel = new CreateTransactionViewModel
        {
            SourceAccountId = 0, // Invalid
            DestinationAccountId = 0, // Invalid
            Amount = 0, // Invalid
            Date = default, // Invalid
            Description = string.Empty // Invalid
        };
        var content = new StringContent(JsonSerializer.Serialize(createTransactionViewModel), Encoding.UTF8, "application/json");

        factory.TransactionServiceMock.Setup(s => s.CreateAsync(It.IsAny<CreateTransactionDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid data"));

        // Act
        var response = await client.PostAsync("/api/transactions", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTransactionById_ShouldReturnTransaction_WhenTransactionExists()
    {
        // Arrange
        var client = factory.CreateClient();
        var transactionId = 1;
        factory.TransactionServiceMock.Setup(s => s.GetByIdAsync(transactionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TransactionResponseDto { Id = transactionId, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now });

        // Act
        var response = await client.GetAsync($"/api/transactions/{transactionId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var transactionResponse = await response.Content.ReadFromJsonAsync<TransactionResponseDto>();
        transactionResponse.ShouldNotBeNull();
        transactionResponse.Id.ShouldBe(transactionId);
    }

    [Fact]
    public async Task GetTransactionById_ShouldReturnNotFound_WhenTransactionDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentTransactionId = 999;
        factory.TransactionServiceMock.Setup(s => s.GetByIdAsync(nonExistentTransactionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as TransactionResponseDto);

        // Act
        var response = await client.GetAsync($"/api/transactions/{nonExistentTransactionId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllTransactions_ShouldReturnPagedTransactions()
    {
        // Arrange
        var client = factory.CreateClient();
        var pagedResult = new PagedResult<TransactionResponseDto>
        {
            Items = new List<TransactionResponseDto>
            {
                new TransactionResponseDto { Id = 1, SourceAccountId = 1, DestinationAccountId = 2, Amount = 100, Date = DateTime.Now },
                new TransactionResponseDto { Id = 2, SourceAccountId = 1, DestinationAccountId = 2, Amount = 200, Date = DateTime.Now }
            },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        factory.TransactionServiceMock.Setup(s => s.GetAllAsync(It.Is<PagedRequest>(p => p.PageNumber == 1 && p.PageSize == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await client.GetAsync("/api/transactions?pageNumber=1&pageSize=10");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<TransactionResponseDto>>();
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeEmpty();
        result.Items.Count.ShouldBe(2);
    }

    [Fact]
    public async Task UpdateTransaction_ShouldReturnNoContent_WhenValidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var transactionId = 1;
        var updateTransactionViewModel = new UpdateTransactionViewModel
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 200,
            Date = DateTime.Now
        };
        var content = new StringContent(JsonSerializer.Serialize(updateTransactionViewModel), Encoding.UTF8, "application/json");

        factory.TransactionServiceMock.Setup(s => s.UpdateAsync(transactionId, It.IsAny<UpdateTransactionDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var response = await client.PutAsync($"/api/transactions/{transactionId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateTransaction_ShouldReturnBadRequest_WhenInvalidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var transactionId = 1;
        var updateTransactionViewModel = new UpdateTransactionViewModel
        {
            SourceAccountId = 0, // Invalid
            DestinationAccountId = 0, // Invalid
            Amount = 0, // Invalid
            Date = default, // Invalid
        };
        var content = new StringContent(JsonSerializer.Serialize(updateTransactionViewModel), Encoding.UTF8, "application/json");

        factory.TransactionServiceMock.Setup(s => s.UpdateAsync(transactionId, It.IsAny<UpdateTransactionDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid data"));

        // Act
        var response = await client.PutAsync($"/api/transactions/{transactionId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTransaction_ShouldReturnNotFound_WhenTransactionDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentTransactionId = 999;
        var updateTransactionViewModel = new UpdateTransactionViewModel
        {
            SourceAccountId = 1,
            DestinationAccountId = 2,
            Amount = 200,
            Date = DateTime.Now
        };
        var content = new StringContent(JsonSerializer.Serialize(updateTransactionViewModel), Encoding.UTF8, "application/json");

        factory.TransactionServiceMock.Setup(s => s.UpdateAsync(nonExistentTransactionId, It.IsAny<UpdateTransactionDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await client.PutAsync($"/api/transactions/{nonExistentTransactionId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTransaction_ShouldReturnNoContent_WhenTransactionExists()
    {
        // Arrange
        var client = factory.CreateClient();
        var transactionId = 1;
        factory.TransactionServiceMock.Setup(s => s.DeleteAsync(transactionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var response = await client.DeleteAsync($"/api/transactions/{transactionId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTransaction_ShouldReturnNotFound_WhenTransactionDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentTransactionId = 999;
        factory.TransactionServiceMock.Setup(s => s.DeleteAsync(nonExistentTransactionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await client.DeleteAsync($"/api/transactions/{nonExistentTransactionId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
