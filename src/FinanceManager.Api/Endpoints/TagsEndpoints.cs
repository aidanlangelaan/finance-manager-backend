using FinanceManager.Application.Tags.Interfaces;
using FinanceManager.Application.Common.Models.Paging;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using FinanceManager.Api.ViewModels.Tag;
using FinanceManager.Api.ViewModels.Tag.Mapping;
using FinanceManager.Api.Common.Filters;

namespace FinanceManager.Api.Endpoints;

public static class TagsEndpoints
{
    public static void MapTagEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup("/api/tags")
            .WithTags("Tags")
            .RequireAuthorization();

        group.MapGet("/", GetAllTagsAsync)
            .WithName("GetAllTags")
            .WithSummary("Get paged tags")
            .WithDescription("Returns a paginated list of all tags owned by the authenticated user.")
            .Produces<PagedResult<TagViewModel>>(StatusCodes.Status200OK, "application/json");

        group.MapGet("/{id:int}", GetTagByIdAsync)
            .WithName("GetTagById")
            .WithSummary("Get tag by ID")
            .WithDescription(
                "Returns the details of a specific tag by its ID if it belongs to the authenticated user.")
            .Produces<TagViewModel>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateTagAsync)
            .WithName("CreateTag")
            .WithSummary("Create a new tag")
            .WithDescription("Creates a new tag for the authenticated user.")
            .AddEndpointFilter<ValidationFilter<CreateTagViewModel>>()
            .Accepts<CreateTagViewModel>("application/json")
            .Produces<int>(StatusCodes.Status201Created, "application/json")
            .ProducesValidationProblem();

        group.MapPut("/{id:int}", UpdateTagAsync)
            .WithName("UpdateTag")
            .WithSummary("Update an tag")
            .WithDescription("Updates an existing tag owned by the authenticated user.")
            .AddEndpointFilter<ValidationFilter<UpdateTagViewModel>>()
            .Accepts<UpdateTagViewModel>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

        group.MapDelete("/{id:int}", DeleteTagAsync)
            .WithName("DeleteTag")
            .WithSummary("Delete an tag")
            .WithDescription("Deletes the specified tag owned by the authenticated user.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllTagsAsync(
        [AsParameters] PagedRequest paging,
        ITagService service,
        TagViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(paging, ct);
        return Results.Ok(new PagedResult<TagViewModel>
        {
            Items = result.Items.Select(mapper.ToViewModel).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        });
    }

    private static async Task<IResult> GetTagByIdAsync(
        int id,
        ITagService service,
        TagViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? Results.NotFound() : Results.Ok(mapper.ToViewModel(result));
    }

    private static async Task<IResult> CreateTagAsync(
        [FromBody] CreateTagViewModel viewModel,
        ITagService service,
        TagViewModelMapper mapper,
        CancellationToken ct)
    {
        var id = await service.CreateAsync(mapper.ToDto(viewModel), ct);
        return Results.Created($"/api/tags/{id}", id);
    }

    private static async Task<IResult> UpdateTagAsync(
        int id,
        [FromBody] UpdateTagViewModel viewModel,
        ITagService service,
        TagViewModelMapper mapper,
        CancellationToken ct)
    {
        var success = await service.UpdateAsync(id, mapper.ToDto(viewModel), ct);
        return success ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> DeleteTagAsync(
        int id,
        ITagService service,
        CancellationToken ct)
    {
        var success = await service.DeleteAsync(id, ct);
        return success ? Results.NoContent() : Results.NotFound();
    }
}
