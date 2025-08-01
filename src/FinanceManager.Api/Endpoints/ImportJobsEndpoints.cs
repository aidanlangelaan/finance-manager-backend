using System.Text.Json;
using FinanceManager.Api.Common.Filters;
using FinanceManager.Api.ViewModels.ImportJob;
using FinanceManager.Api.ViewModels.ImportJob.Mapping;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Application.ImportJobs.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Endpoints;

public static class ImportJobsEndpoints
{
    public static void MapImportJobEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup("/api/imports")
            .WithTags("Imports")
            .RequireAuthorization();

        group.MapGet("/", GetAllImportJobsAsync)
            .WithName("GetAllImportJobs")
            .WithSummary("Get all paged import jobs")
            .WithDescription("Returns a paginated list of all import jobs for the authenticated user.")
            .Produces<IEnumerable<ImportJobViewModel>>();

        group.MapGet("/{id:int}", GetImportJobByIdAsync)
            .WithName("GetImportJobById")
            .WithSummary("Get import job by ID")
            .WithDescription("Returns details of an import job including any errors.")
            .Produces<ImportJobDetailsViewModel>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateImportJobAsync)
            .WithName("CreateImportJob")
            .WithSummary("Create a new import job")
            .WithDescription("Uploads a CSV file and starts a background import job.")
            .Accepts<CreateImportJobViewModel>("multipart/form-data")
            .Produces<int>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPatch("/{id:int}", UpdateImportJobAsync)
            .WithName("UpdateImportJob")
            .WithSummary("Update an import job")
            .WithDescription("Updates an existing import job owned by the authenticated user.")
            .AddEndpointFilter<ValidationFilter<UpdateImportJobViewModel>>()
            .Accepts<UpdateImportJobViewModel>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> CreateImportJobAsync(
        [FromForm] IFormFile file,
        [FromForm] string originalFileName,
        [FromForm] string mappingProfileJson,
        [FromForm] bool notifyOnCompletion,
        IImportJobService service,
        ImportJobViewModelMapper mapper,
        CancellationToken ct)
    {
        if (file.Length == 0)
            return Results.BadRequest("File is missing or empty.");

        if (string.IsNullOrWhiteSpace(originalFileName))
            return Results.BadRequest("Original file name is required.");

        if (string.IsNullOrWhiteSpace(mappingProfileJson))
            return Results.BadRequest("Mapping profile JSON is required.");

        JsonDocument mappingProfile;

        try
        {
            mappingProfile = JsonDocument.Parse(mappingProfileJson);
        }
        catch (JsonException)
        {
            return Results.BadRequest("Mapping profile is not valid JSON.");
        }

        var dto = mapper.ToCreateDto(originalFileName, mappingProfile, notifyOnCompletion);

        await using var stream = file.OpenReadStream();
        var id = await service.CreateAsync(dto, stream, ct);

        return Results.Created($"/api/imports/{id}", id);
    }

    private static async Task<IResult> GetAllImportJobsAsync(
        [AsParameters] PagedRequest paging,
        IImportJobService service,
        ImportJobViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(paging, ct);
        return Results.Ok(new PagedResult<ImportJobViewModel>
        {
            Items = result.Items.Select(mapper.ToViewModel).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        });
    }

    private static async Task<IResult> GetImportJobByIdAsync(
        int id,
        IImportJobService service,
        ImportJobViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? Results.NotFound() : Results.Ok(mapper.ToDetailsViewModel(result));
    }

    private static async Task<IResult> UpdateImportJobAsync(
        int id,
        [FromBody] UpdateImportJobViewModel viewModel,
        IImportJobService service,
        ImportJobViewModelMapper mapper,
        CancellationToken ct)
    {
        var success = await service.UpdateAsync(id, mapper.ToUpdateDto(viewModel), ct);
        return success ? Results.NoContent() : Results.NotFound();
    }
}
