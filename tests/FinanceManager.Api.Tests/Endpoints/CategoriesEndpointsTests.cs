using System.Net;
using System.Text;
using System.Text.Json;
using FinanceManager.Api.Tests.Common;
using FinanceManager.Api.ViewModels.Category;
using FinanceManager.Application.Categories.Dtos;
using FinanceManager.Application.Common.Models.Paging;
using Shouldly;
using System.Net.Http.Json;
using Moq;

namespace FinanceManager.Api.Tests.Endpoints;

public class CategoriesEndpointsTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task CreateCategory_ShouldReturnCreatedCategory_WhenValidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var createCategoryViewModel = new CreateCategoryViewModel
        {
            Name = "Test Category"
        };
        var content = new StringContent(JsonSerializer.Serialize(createCategoryViewModel), Encoding.UTF8, "application/json");

        var createdCategoryId = 1;
        factory.CategoryServiceMock.Setup(s => s.CreateAsync(It.IsAny<CreateCategoryDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdCategoryId);
        factory.CategoryServiceMock.Setup(s => s.GetByIdAsync(createdCategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategoryResponseDto
            {
                Id = createdCategoryId,
                Name = createCategoryViewModel.Name
            });

        // Act
        var response = await client.PostAsync("/api/categories", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var categoryId = await response.Content.ReadFromJsonAsync<int>();
        categoryId.ShouldBe(createdCategoryId);

        var getResponse = await client.GetAsync($"/api/categories/{categoryId}");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var categoryResponse = await getResponse.Content.ReadFromJsonAsync<CategoryResponseDto>();
        categoryResponse.ShouldNotBeNull();
        categoryResponse.Name.ShouldBe(createCategoryViewModel.Name);
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnBadRequest_WhenInvalidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var createCategoryViewModel = new CreateCategoryViewModel
        {
            Name = string.Empty, // Invalid name
        };
        var content = new StringContent(JsonSerializer.Serialize(createCategoryViewModel), Encoding.UTF8, "application/json");

        factory.CategoryServiceMock.Setup(s => s.CreateAsync(It.IsAny<CreateCategoryDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid data"));

        // Act
        var response = await client.PostAsync("/api/categories", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetCategoryById_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var client = factory.CreateClient();
        var categoryId = 1;
        factory.CategoryServiceMock.Setup(s => s.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CategoryResponseDto { Id = categoryId, Name = "Test Category" });

        // Act
        var response = await client.GetAsync($"/api/categories/{categoryId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var categoryResponse = await response.Content.ReadFromJsonAsync<CategoryResponseDto>();
        categoryResponse.ShouldNotBeNull();
        categoryResponse.Id.ShouldBe(categoryId);
    }

    [Fact]
    public async Task GetCategoryById_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentCategoryId = 999;
        factory.CategoryServiceMock.Setup(s => s.GetByIdAsync(nonExistentCategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as CategoryResponseDto);

        // Act
        var response = await client.GetAsync($"/api/categories/{nonExistentCategoryId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllCategories_ShouldReturnPagedCategories()
    {
        // Arrange
        var client = factory.CreateClient();
        var pagedResult = new PagedResult<CategoryResponseDto>
        {
            Items = new List<CategoryResponseDto>
            {
                new CategoryResponseDto { Id = 1, Name = "Category 1" },
                new CategoryResponseDto { Id = 2, Name = "Category 2" }
            },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        factory.CategoryServiceMock.Setup(s => s.GetAllAsync(It.Is<PagedRequest>(p => p.PageNumber == 1 && p.PageSize == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await client.GetAsync("/api/categories?pageNumber=1&pageSize=10");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<CategoryResponseDto>>();
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeEmpty();
        result.Items.Count.ShouldBe(2);
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturnNoContent_WhenValidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var categoryId = 1;
        var updateCategoryViewModel = new UpdateCategoryViewModel
        {
            Name = "Updated Category Name"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateCategoryViewModel), Encoding.UTF8, "application/json");

        factory.CategoryServiceMock.Setup(s => s.UpdateAsync(categoryId, It.IsAny<UpdateCategoryDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var response = await client.PutAsync($"/api/categories/{categoryId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturnBadRequest_WhenInvalidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var categoryId = 1;
        var updateCategoryViewModel = new UpdateCategoryViewModel
        {
            Name = string.Empty, // Invalid name
        };
        var content = new StringContent(JsonSerializer.Serialize(updateCategoryViewModel), Encoding.UTF8, "application/json");

        factory.CategoryServiceMock.Setup(s => s.UpdateAsync(categoryId, It.IsAny<UpdateCategoryDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid data"));

        // Act
        var response = await client.PutAsync($"/api/categories/{categoryId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentCategoryId = 999;
        var updateCategoryViewModel = new UpdateCategoryViewModel
        {
            Name = "Updated Category Name"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateCategoryViewModel), Encoding.UTF8, "application/json");

        factory.CategoryServiceMock.Setup(s => s.UpdateAsync(nonExistentCategoryId, It.IsAny<UpdateCategoryDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await client.PutAsync($"/api/categories/{nonExistentCategoryId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnNoContent_WhenCategoryExists()
    {
        // Arrange
        var client = factory.CreateClient();
        var categoryId = 1;
        factory.CategoryServiceMock.Setup(s => s.DeleteAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var response = await client.DeleteAsync($"/api/categories/{categoryId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentCategoryId = 999;
        factory.CategoryServiceMock.Setup(s => s.DeleteAsync(nonExistentCategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await client.DeleteAsync($"/api/categories/{nonExistentCategoryId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
