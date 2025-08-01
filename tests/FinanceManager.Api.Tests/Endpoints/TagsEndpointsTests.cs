using System.Net;
using System.Text;
using System.Text.Json;
using FinanceManager.Api.Tests.Common;
using FinanceManager.Api.ViewModels.Tag;
using FinanceManager.Application.Tags.Dtos;
using FinanceManager.Application.Common.Models.Paging;
using Shouldly;
using System.Net.Http.Json;
using Moq;

namespace FinanceManager.Api.Tests.Endpoints;

public class TagsEndpointsTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task CreateTag_ShouldReturnCreatedTag_WhenValidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var createTagViewModel = new CreateTagViewModel
        {
            Name = "Test Tag"
        };
        var content = new StringContent(JsonSerializer.Serialize(createTagViewModel), Encoding.UTF8, "application/json");

        var createdTagId = 1;
        factory.TagServiceMock.Setup(s => s.CreateAsync(It.IsAny<CreateTagDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdTagId);
        factory.TagServiceMock.Setup(s => s.GetByIdAsync(createdTagId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TagResponseDto
            {
                Id = createdTagId,
                Name = createTagViewModel.Name
            });

        // Act
        var response = await client.PostAsync("/api/tags", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        var tagId = await response.Content.ReadFromJsonAsync<int>();
        tagId.ShouldBe(createdTagId);

        var getResponse = await client.GetAsync($"/api/tags/{tagId}");
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        var tagResponse = await getResponse.Content.ReadFromJsonAsync<TagResponseDto>();
        tagResponse.ShouldNotBeNull();
        tagResponse.Name.ShouldBe(createTagViewModel.Name);
    }

    [Fact]
    public async Task CreateTag_ShouldReturnBadRequest_WhenInvalidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var createTagViewModel = new CreateTagViewModel
        {
            Name = string.Empty, // Invalid name
        };
        var content = new StringContent(JsonSerializer.Serialize(createTagViewModel), Encoding.UTF8, "application/json");

        factory.TagServiceMock.Setup(s => s.CreateAsync(It.IsAny<CreateTagDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid data"));

        // Act
        var response = await client.PostAsync("/api/tags", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetTagById_ShouldReturnTag_WhenTagExists()
    {
        // Arrange
        var client = factory.CreateClient();
        var tagId = 1;
        factory.TagServiceMock.Setup(s => s.GetByIdAsync(tagId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TagResponseDto { Id = tagId, Name = "Test Tag" });

        // Act
        var response = await client.GetAsync($"/api/tags/{tagId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var tagResponse = await response.Content.ReadFromJsonAsync<TagResponseDto>();
        tagResponse.ShouldNotBeNull();
        tagResponse.Id.ShouldBe(tagId);
    }

    [Fact]
    public async Task GetTagById_ShouldReturnNotFound_WhenTagDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentTagId = 999;
        factory.TagServiceMock.Setup(s => s.GetByIdAsync(nonExistentTagId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as TagResponseDto);

        // Act
        var response = await client.GetAsync($"/api/tags/{nonExistentTagId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllTags_ShouldReturnPagedTags()
    {
        // Arrange
        var client = factory.CreateClient();
        var pagedResult = new PagedResult<TagResponseDto>
        {
            Items = new List<TagResponseDto>
            {
                new TagResponseDto { Id = 1, Name = "Tag 1" },
                new TagResponseDto { Id = 2, Name = "Tag 2" }
            },
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10
        };
        factory.TagServiceMock.Setup(s => s.GetAllAsync(It.Is<PagedRequest>(p => p.PageNumber == 1 && p.PageSize == 10), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        var response = await client.GetAsync("/api/tags?pageNumber=1&pageSize=10");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<TagResponseDto>>();
        result.ShouldNotBeNull();
        result.Items.ShouldNotBeEmpty();
        result.Items.Count.ShouldBe(2);
    }

    [Fact]
    public async Task UpdateTag_ShouldReturnNoContent_WhenValidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var tagId = 1;
        var updateTagViewModel = new UpdateTagViewModel
        {
            Name = "Updated Tag Name"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateTagViewModel), Encoding.UTF8, "application/json");

        factory.TagServiceMock.Setup(s => s.UpdateAsync(tagId, It.IsAny<UpdateTagDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var response = await client.PutAsync($"/api/tags/{tagId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateTag_ShouldReturnBadRequest_WhenInvalidDataProvided()
    {
        // Arrange
        var client = factory.CreateClient();
        var tagId = 1;
        var updateTagViewModel = new UpdateTagViewModel
        {
            Name = string.Empty, // Invalid name
        };
        var content = new StringContent(JsonSerializer.Serialize(updateTagViewModel), Encoding.UTF8, "application/json");

        factory.TagServiceMock.Setup(s => s.UpdateAsync(tagId, It.IsAny<UpdateTagDto>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FluentValidation.ValidationException("Invalid data"));

        // Act
        var response = await client.PutAsync($"/api/tags/{tagId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateTag_ShouldReturnNotFound_WhenTagDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentTagId = 999;
        var updateTagViewModel = new UpdateTagViewModel
        {
            Name = "Updated Tag Name"
        };
        var content = new StringContent(JsonSerializer.Serialize(updateTagViewModel), Encoding.UTF8, "application/json");

        factory.TagServiceMock.Setup(s => s.UpdateAsync(nonExistentTagId, It.IsAny<UpdateTagDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await client.PutAsync($"/api/tags/{nonExistentTagId}", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteTag_ShouldReturnNoContent_WhenTagExists()
    {
        // Arrange
        var client = factory.CreateClient();
        var tagId = 1;
        factory.TagServiceMock.Setup(s => s.DeleteAsync(tagId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var response = await client.DeleteAsync($"/api/tags/{tagId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteTag_ShouldReturnNotFound_WhenTagDoesNotExist()
    {
        // Arrange
        var client = factory.CreateClient();
        var nonExistentTagId = 999;
        factory.TagServiceMock.Setup(s => s.DeleteAsync(nonExistentTagId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var response = await client.DeleteAsync($"/api/tags/{nonExistentTagId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
