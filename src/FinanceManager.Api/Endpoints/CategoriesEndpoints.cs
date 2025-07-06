using FinanceManager.Application.Categories.Interfaces;
using FinanceManager.Application.Common.Models.Paging;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using FinanceManager.Api.ViewModels.Category;
using FinanceManager.Api.ViewModels.Category.Mapping;

namespace FinanceManager.Api.Endpoints;

public static class CategoriesEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup("/api/categories")
            .WithTags("Categories")
            .RequireAuthorization();

        group.MapGet("/", GetAllCategoriesAsync)
            .WithName("GetAllCategories")
            .WithSummary("Get paged categories")
            .WithDescription("Returns a paginated list of all categories owned by the authenticated user.")
            .Produces<PagedResult<CategoryViewModel>>(StatusCodes.Status200OK, "application/json");

        group.MapGet("/{id:int}", GetCategoryByIdAsync)
            .WithName("GetCategoryById")
            .WithSummary("Get category by ID")
            .WithDescription(
                "Returns the details of a specific category by its ID if it belongs to the authenticated user.")
            .Produces<CategoryViewModel>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateCategoryAsync)
            .WithName("CreateCategory")
            .WithSummary("Create a new category")
            .WithDescription("Creates a new category for the authenticated user.")
            .Accepts<CreateCategoryViewModel>("application/json")
            .Produces<int>(StatusCodes.Status201Created, "application/json")
            .ProducesValidationProblem();

        group.MapPut("/{id:int}", UpdateCategoryAsync)
            .WithName("UpdateCategory")
            .WithSummary("Update an category")
            .WithDescription("Updates an existing category owned by the authenticated user.")
            .Accepts<UpdateCategoryViewModel>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

        group.MapDelete("/{id:int}", DeleteCategoryAsync)
            .WithName("DeleteCategory")
            .WithSummary("Delete an category")
            .WithDescription("Deletes the specified category owned by the authenticated user.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllCategoriesAsync(
        [AsParameters] PagedRequest paging,
        ICategoryService service,
        CategoryViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(paging, ct);
        return Results.Ok(new PagedResult<CategoryViewModel>
        {
            Items = result.Items.Select(mapper.ToViewModel).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        });
    }

    private static async Task<IResult> GetCategoryByIdAsync(
        int id,
        ICategoryService service,
        CategoryViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? Results.NotFound() : Results.Ok(mapper.ToViewModel(result));
    }

    private static async Task<IResult> CreateCategoryAsync(
        [FromBody] CreateCategoryViewModel viewModel,
        IValidator<CreateCategoryViewModel> validator,
        ICategoryService service,
        CategoryViewModelMapper mapper,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(viewModel, ct);
        if (!validation.IsValid)
            return Results.ValidationProblem(validation.ToDictionary());

        var id = await service.CreateAsync(mapper.ToDto(viewModel), ct);
        return Results.Created($"/api/categories/{id}", id);
    }

    private static async Task<IResult> UpdateCategoryAsync(
        int id,
        [FromBody] UpdateCategoryViewModel viewModel,
        IValidator<UpdateCategoryViewModel> validator,
        ICategoryService service,
        CategoryViewModelMapper mapper,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(viewModel, ct);
        if (!validation.IsValid)
            return Results.ValidationProblem(validation.ToDictionary());

        var success = await service.UpdateAsync(id, mapper.ToDto(viewModel), ct);
        return success ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> DeleteCategoryAsync(
        int id,
        ICategoryService service,
        CancellationToken ct)
    {
        var success = await service.DeleteAsync(id, ct);
        return success ? Results.NoContent() : Results.NotFound();
    }
}
