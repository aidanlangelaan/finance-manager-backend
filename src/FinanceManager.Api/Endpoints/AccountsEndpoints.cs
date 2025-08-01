using FinanceManager.Application.Accounts.Interfaces;
using FinanceManager.Application.Common.Models.Paging;
using Microsoft.AspNetCore.Mvc;
using FinanceManager.Api.ViewModels.Account;
using FinanceManager.Api.ViewModels.Account.Mapping;
using FinanceManager.Api.Common.Filters;

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
            .Produces<PagedResult<AccountViewModel>>(StatusCodes.Status200OK, "application/json");

        group.MapGet("/{id:int}", GetAccountByIdAsync)
            .WithName("GetAccountById")
            .WithSummary("Get account by ID")
            .WithDescription(
                "Returns the details of a specific account by its ID if it belongs to the authenticated user.")
            .Produces<AccountViewModel>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateAccountAsync)
            .WithName("CreateAccount")
            .WithSummary("Create a new account")
            .WithDescription("Creates a new account for the authenticated user.")
            .AddEndpointFilter<ValidationFilter<CreateAccountViewModel>>()
            .Accepts<CreateAccountViewModel>("application/json")
            .Produces<int>(StatusCodes.Status201Created, "application/json")
            .ProducesValidationProblem();

        group.MapPut("/{id:int}", UpdateAccountAsync)
            .WithName("UpdateAccount")
            .WithSummary("Update an account")
            .WithDescription("Updates an existing account owned by the authenticated user.")
            .AddEndpointFilter<ValidationFilter<UpdateAccountViewModel>>()
            .Accepts<UpdateAccountViewModel>("application/json")
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
        [FromServices] IAccountService service,
        [FromServices] AccountViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(paging, ct);
        return Results.Ok(new PagedResult<AccountViewModel>
        {
            Items = result.Items.Select(mapper.ToViewModel).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        });
    }

    private static async Task<IResult> GetAccountByIdAsync(
        int id,
        [FromServices] IAccountService service,
        [FromServices] AccountViewModelMapper mapper,
        CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? Results.NotFound() : Results.Ok(mapper.ToViewModel(result));
    }

    private static async Task<IResult> CreateAccountAsync(
        [FromBody] CreateAccountViewModel viewModel,
        [FromServices] IAccountService service,
        [FromServices] AccountViewModelMapper mapper,
        CancellationToken ct)
    {
        var id = await service.CreateAsync(mapper.ToDto(viewModel), ct);
        return Results.Created($"/api/accounts/{id}", id);
    }

    private static async Task<IResult> UpdateAccountAsync(
        int id,
        [FromBody] UpdateAccountViewModel viewModel,
        [FromServices] IAccountService service,
        [FromServices] AccountViewModelMapper mapper,
        CancellationToken ct)
    {
        var success = await service.UpdateAsync(id, mapper.ToDto(viewModel), ct);
        return success ? Results.NoContent() : Results.NotFound();
    }

    private static async Task<IResult> DeleteAccountAsync(
        int id,
        [FromServices] IAccountService service,
        CancellationToken ct)
    {
        var success = await service.DeleteAsync(id, ct);
        return success ? Results.NoContent() : Results.NotFound();
    }
}
