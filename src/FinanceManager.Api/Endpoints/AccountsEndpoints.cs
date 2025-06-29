using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Accounts.Interfaces;
using FinanceManager.Application.Common.Models.Paging;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Endpoints;

public static class AccountsEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var group = endpointRouteBuilder
            .MapGroup("/api/accounts")
            .WithTags("Accounts")
            .RequireAuthorization();

        group.MapGet("/", GetAllAccountsAsync)
            .WithName("GetAllAccounts")
            .WithSummary("Get paged accounts")
            .WithDescription("Returns a paginated list of all accounts owned by the authenticated user.")
            .Produces<PagedResult<AccountResponseDto>>(StatusCodes.Status200OK, "application/json");

        group.MapGet("/{id:int}", GetAccountByIdAsync)
            .WithName("GetAccountById")
            .WithSummary("Get account by ID")
            .WithDescription("Returns the details of a specific account by its ID if it belongs to the authenticated user.")
            .Produces<AccountResponseDto>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateAccountAsync)
            .WithName("CreateAccount")
            .WithSummary("Create a new account")
            .WithDescription("Creates a new account for the authenticated user.")
            .Accepts<CreateAccountDto>("application/json")
            .Produces<int>(StatusCodes.Status201Created, "application/json")
            .ProducesValidationProblem();

        group.MapPut("/{id:int}", UpdateAccountAsync)
            .WithName("UpdateAccount")
            .WithSummary("Update an account")
            .WithDescription("Updates an existing account owned by the authenticated user.")
            .Accepts<UpdateAccountDto>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

        group.MapDelete("/{id:int}", DeleteAccountAsync)
            .WithName("DeleteAccount")
            .WithSummary("Delete an account")
            .WithDescription("Deletes the specified account owned by the authenticated user.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAllAccountsAsync(
        [AsParameters] PagedRequest paging,
        IAccountService service,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(paging, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetAccountByIdAsync(
        int id,
        IAccountService service,
        CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> CreateAccountAsync(
        [FromBody] CreateAccountDto dto,
        IValidator<CreateAccountDto> validator,
        IAccountService service,
        CancellationToken ct)
    {
        var validation = await validator.ValidateAsync(dto, ct);
        if (!validation.IsValid)
            return Results.ValidationProblem(validation.ToDictionary());

        var id = await service.CreateAsync(dto, ct);
        return Results.Created($"/api/accounts/{id}", id);
    }

    private static async Task<IResult> UpdateAccountAsync(
        int id,
        [FromBody] UpdateAccountDto dto,
        IValidator<UpdateAccountDto> validator,
        IAccountService service,
        CancellationToken ct)
    {
        if (id != dto.Id)
            return Results.BadRequest(new { error = "The ID in the URL does not match the ID in the payload." });

        var validation = await validator.ValidateAsync(dto, ct);
        if (!validation.IsValid)
            return Results.ValidationProblem(validation.ToDictionary());

        var success = await service.UpdateAsync(dto, ct);
        return success ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> DeleteAccountAsync(
        int id,
        IAccountService service,
        CancellationToken ct)
    {
        var success = await service.DeleteAsync(id, ct);
        return success ? Results.NoContent() : Results.NotFound();
    }
}
