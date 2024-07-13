using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionController(ITransactionService transactionService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets all transactions
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/transaction
    ///
    /// </remarks>
    /// <returns>List of transactions</returns>
    /// <response code="200">Transactions have been retrieved</response>
    /// <response code="400">Failed to process request</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetTransactionViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        var transactions = await transactionService.GetAll();
        return Ok(transactions);
    }

    /// <summary>
    /// Gets a single transaction by its id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/transaction/1
    ///
    /// </remarks>
    /// <returns>A single transaction</returns>
    /// <response code="200">Transaction for given id has been retrieved</response>
    /// <response code="404">Transaction not found for given id</response>
    /// <response code="400">Failed to process request</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetTransactionViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(int id)
    {
        var transaction = await transactionService.GetById(id);
        if (transaction != null)
        {
            return Ok(transaction);
        }

        return NotFound();
    }

    /// <summary>
    /// Creates a transaction
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/transaction/1
    ///     {
    ///         "amount": 0,
    ///         "fromAccountId": 0,
    ///         "toAccountId": 0,
    ///         "date": "2021-10-15T22:32:25.405Z",
    ///         "description": "string",
    ///         "categoryId": 0
    ///     }
    ///
    /// </remarks>
    /// <returns>A transaction</returns>
    /// <response code="201">Transaction has been created</response>
    /// <response code="400">Request is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(GetTransactionViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateTransactionViewModel model)
    {
        var transaction = await transactionService.Create(mapper.Map<CreateTransactionDTO>(model));
        return Created($"api/transaction/{transaction.Id}", transaction);
    }

    /// <summary>
    /// Updates a transaction
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/transaction
    ///     {
    ///         "id": 0
    ///         "amount": 0,
    ///         "fromAccountId": 0,
    ///         "toAccountId": 0,
    ///         "date": "2021-10-15T22:32:25.405Z",
    ///         "description": "string",
    ///         "categoryId": 0
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Transaction has been updated</response>
    /// <response code="400">Request is invalid</response>
    [HttpPut]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(UpdateTransactionViewModel model)
    {
        await transactionService.Update(mapper.Map<UpdateTransactionDTO>(model));
        return Ok();
    }

    /// <summary>
    /// Deletes a transaction
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/transaction/1
    ///
    /// </remarks>
    /// <response code="200">Transaction with given id has been deleted</response>
    /// <response code="404">Transaction not found for given id</response>
    /// <response code="400">Request is invalid</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var transaction = await transactionService.GetById(id);
        if (transaction == null) return NotFound();
        await transactionService.Delete(transaction.Id);
        return Ok();
    }
}