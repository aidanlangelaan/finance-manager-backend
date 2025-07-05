using FinanceManager.Api.ViewModels.Transaction;
using FinanceManager.Api.ViewModels.Transaction.Mapping;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Application.Transactions.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Endpoints;

public static class TransactionsEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup("/api/transactions")
            .WithTags("Transactions")
            .RequireAuthorization();

        group.MapGet("/", GetAllTransactionsAsync)
            .WithName("GetAllTransactions")
            .WithSummary("Get paged transactions")
            .WithDescription("Returns a paginated list of all transactions owned by the authenticated user.")
            .Produces<PagedResult<TransactionViewModel>>(StatusCodes.Status200OK, "application/json");

        group.MapGet("/{id:int}", GetTransactionByIdAsync)
            .WithName("GetTransactionById")
            .WithSummary("Get transaction by ID")
            .WithDescription(
                "Returns the details of a specific transaction by its ID if it belongs to the authenticated user.")
            .Produces<TransactionViewModel>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateTransactionAsync)
            .WithName("CreateTransaction")
            .WithSummary("Create a new transaction")
            .WithDescription("Creates a new transaction for the authenticated user.")
            .Accepts<CreateTransactionViewModel>("application/json")
            .Produces<int>(StatusCodes.Status201Created, "application/json")
            .ProducesValidationProblem();

        group.MapPut("/{id:int}", UpdateTransactionAsync)
            .WithName("UpdateTransaction")
            .WithSummary("Update an transaction")
            .WithDescription("Updates an existing transaction owned by the authenticated user.")
            .Accepts<UpdateTransactionViewModel>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

        group.MapDelete("/{id:int}", DeleteTransactionAsync)
            .WithName("DeleteTransaction")
            .WithSummary("Delete an transaction")
            .WithDescription("Deletes the specified transaction owned by the authenticated user.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllTransactionsAsync(
        [AsParameters] PagedRequest paging,
        ITransactionService service,
        TransactionViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(paging, ct);
        return Results.Ok(new PagedResult<TransactionViewModel>
        {
            Items = result.Items.Select(mapper.ToViewModel).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        });
    }

    private static async Task<IResult> GetTransactionByIdAsync(
        int id,
        ITransactionService service,
        TransactionViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? Results.NotFound() : Results.Ok(mapper.ToViewModel(result));
    }

    private static async Task<IResult> CreateTransactionAsync(
        [FromBody] CreateTransactionViewModel viewModel,
        IValidator<CreateTransactionViewModel> validator,
        ITransactionService service,
        TransactionViewModelMapper mapper,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(viewModel, ct);
        if (!validation.IsValid)
            return Results.ValidationProblem(validation.ToDictionary());

        var id = await service.CreateAsync(mapper.ToDto(viewModel), ct);
        return Results.Created($"/api/transactions/{id}", id);
    }

    private static async Task<IResult> UpdateTransactionAsync(
        int id,
        [FromBody] UpdateTransactionViewModel viewModel,
        IValidator<UpdateTransactionViewModel> validator,
        ITransactionService service,
        TransactionViewModelMapper mapper,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(viewModel, ct);
        if (!validation.IsValid)
            return Results.ValidationProblem(validation.ToDictionary());

        var success = await service.UpdateAsync(id, mapper.ToDto(viewModel), ct);
        return success ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> DeleteTransactionAsync(
        int id,
        ITransactionService service,
        CancellationToken ct)
    {
        var success = await service.DeleteAsync(id, ct);
        return success ? Results.NoContent() : Results.NotFound();
    }
}
