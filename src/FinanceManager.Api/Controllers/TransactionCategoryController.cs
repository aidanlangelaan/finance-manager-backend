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
public class TransactionCategoryController(ITransactionService transactionService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Updates the category of a transaction and optionally applies it to similar transactions
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/transactioncategory/1
    ///     {
    ///         "transactionId": 0
    ///         "categoryId": 0,
    ///         "applyToSimilarTransactions": false
    ///     }
    ///
    /// </remarks>
    /// <returns>The amount of transactions that have been updated</returns>
    /// <response code="200">Transactions have been updated</response>
    /// <response code="404">Transaction not found for given id</response>
    /// <response code="400">Request is invalid</response>
    [HttpPut]
    [ProducesResponseType(typeof(IEnumerable<GetTransactionViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignCategoryToTransaction(AssignCategoryToTransactionViewModel model)
    {
        var result = await transactionService.AssignCategoryToTransaction(mapper.Map<AssignCategoryToTransactionDTO>(model));
        if (result.Count == 0)
        {
            return NotFound();
        }
        
        return Ok(mapper.Map<List<GetTransactionViewModel>>(result));
    }
}