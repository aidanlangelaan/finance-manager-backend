using System.Net;
using System.Text;
using System.Text.Json;
using FinanceManager.Api.Tests.Common;
using FinanceManager.Api.ViewModels.Account;
using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Common.Models.Paging;
using Shouldly;
using System.Net.Http.Json;
using Moq;

namespace FinanceManager.Api.Tests.Endpoints;

public class AccountsEndpointsTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task CreateAccount_ShouldReturnCreatedAccount_WhenValidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var createAccountViewModel = new CreateAccountViewModel
        {
            Name = "Test",
            Type = Domain.Enums.AccountType.Asset,
            CurrentBalance = 1000
        };
        var content = new StringContent(JsonSerializer.Serialize(createAccountViewModel), Encoding.UTF8, "application/json");

        var createdAccountId = 1;
        factory.AccountServiceMock.Setup(s => s.CreateAsync(It.IsAny<CreateAccountDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdAccountId);
        factory.AccountServiceMock.Setup(s => s.GetByIdAsync(createdAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AccountResponseDto
            {
                Id = createdAccountId,
                Name = createAccountViewModel.Name,
                Type = createAccountViewModel.Type,
                CurrentBalance = createAccountViewModel.CurrentBalance
            });

        // Act
        var response = await client.PostAsync("/api/accounts", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var accountId = await response.Content.ReadFromJsonAsync<int>();
        accountId.ShouldBe(createdAccountId);

        var getResponse = await client.GetAsync($"/api/accounts/{accountId}");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var accountResponse = await getResponse.Content.ReadFromJsonAsync<AccountResponseDto>();
        accountResponse.ShouldNotBeNull();
        accountResponse.Name.ShouldBe(createAccountViewModel.Name);
        accountResponse.Type.ShouldBe(createAccountViewModel.Type);
        accountResponse.CurrentBalance.ShouldBe(createAccountViewModel.CurrentBalance);
    }

    [Fact]
    public async Task CreateAccount_ShouldReturnBadRequest_WhenInvalidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var createAccountViewModel = new CreateAccountViewModel
        {
            Name = string.Empty, // Invalid name
            Type = Domain.Enums.AccountType.Asset,
            CurrentBalance = 1000
        };
        var content = new StringContent(JsonSerializer.Serialize(createAccountViewModel), Encoding.UTF8, "application/json");

        factory.AccountServiceMock.Setup(s => s.CreateAsync(It.IsAny<CreateAccountDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid data"));

        // Act
        var response = await client.PostAsync("/api/accounts", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAccountById_ShouldReturnAccount_WhenAccountExists()
    {
        // Arrange
        var client = factory.CreateClient();
        var accountId = 1;
        factory.AccountServiceMock.Setup(s => s.GetByIdAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AccountResponseDto { Id = accountId, Name = "Test", Type = Domain.Enums.AccountType.Asset, CurrentBalance = 1000 });

        // Act
        var response = await client.GetAsync($"/api/accounts/{accountId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var accountResponse = await response.Content.ReadFromJsonAsync<AccountResponseDto>();
        accountResponse.ShouldNotBeNull();
        accountResponse.Id.ShouldBe(accountId);
    }

    [Fact]
    public async Task GetAccountById_ShouldReturnNotFound_WhenAccountDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentAccountId = 999;
        factory.AccountServiceMock.Setup(s => s.GetByIdAsync(nonExistentAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as AccountResponseDto);

        // Act
        var response = await client.GetAsync($"/api/accounts/{nonExistentAccountId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllAccounts_ShouldReturnPagedAccounts()
    {
        // Arrange
        var client = factory.CreateClient();
        var pagedResult = new PagedResult<AccountResponseDto>
        {
            Items = new List<AccountResponseDto>
            {
                new AccountResponseDto { Id = 1, Name = "Account 1", Type = Domain.Enums.AccountType.Asset, CurrentBalance = 100 },
                new AccountResponseDto { Id = 2, Name = "Account 2", Type = Domain.Enums.AccountType.Asset, CurrentBalance = 200 }
            },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        factory.AccountServiceMock.Setup(s => s.GetAllAsync(It.IsAny<PagedRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await client.GetAsync("/api/accounts?pageNumber=1&pageSize=10");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<AccountResponseDto>>();
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeEmpty();
        result.Items.Count.ShouldBe(2);
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturnNoContent_WhenValidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var accountId = 1;
        var updateAccountViewModel = new UpdateAccountViewModel
        {
            Name = "Updated Account Name",
            Type = Domain.Enums.AccountType.Asset,
            Description = "Updated description"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateAccountViewModel), Encoding.UTF8, "application/json");

        factory.AccountServiceMock.Setup(s => s.UpdateAsync(accountId, It.IsAny<UpdateAccountDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var response = await client.PutAsync($"/api/accounts/{accountId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturnBadRequest_WhenInvalidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var accountId = 1;
        var updateAccountViewModel = new UpdateAccountViewModel
        {
            Name = string.Empty, // Invalid name
            Type = Domain.Enums.AccountType.Asset
        };
        var content = new StringContent(JsonSerializer.Serialize(updateAccountViewModel), Encoding.UTF8, "application/json");

        factory.AccountServiceMock.Setup(s => s.UpdateAsync(accountId, It.IsAny<UpdateAccountDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid data"));

        // Act
        var response = await client.PutAsync($"/api/accounts/{accountId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturnNotFound_WhenAccountDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentAccountId = 999;
        var updateAccountViewModel = new UpdateAccountViewModel
        {
            Name = "Updated Account Name",
            Type = Domain.Enums.AccountType.Asset
        };
        var content = new StringContent(JsonSerializer.Serialize(updateAccountViewModel), Encoding.UTF8, "application/json");

        factory.AccountServiceMock.Setup(s => s.UpdateAsync(nonExistentAccountId, It.IsAny<UpdateAccountDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await client.PutAsync($"/api/accounts/{nonExistentAccountId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnNoContent_WhenAccountExists()
    {
        // Arrange
        var client = factory.CreateClient();
        var accountId = 1;
        factory.AccountServiceMock.Setup(s => s.DeleteAsync(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var response = await client.DeleteAsync($"/api/accounts/{accountId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnNotFound_WhenAccountDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentAccountId = 999;
        factory.AccountServiceMock.Setup(s => s.DeleteAsync(nonExistentAccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await client.DeleteAsync($"/api/accounts/{nonExistentAccountId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
