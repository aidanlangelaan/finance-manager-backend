using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FinanceManager.Api.Models.Import;
using FinanceManager.Data.Enums;

namespace FinanceManager.Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImportController(IImportService importService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Imports a single csv file containing transactions
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/import/transactions
    ///
    /// </remarks>
    /// <response code="200">File has been imported and is ready to be processed</response>
    /// <response code="400">Failed to import file</response>          
    [HttpPost("transactions")]
    [ProducesResponseType(typeof(List<CsvImportResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportTransactions(IFormFile file, BankType bankType)
    {
        var viewModel = new ImportTransactionsViewModel
        {
            File = file,
            Bank = bankType
        };
        
        var result = await importService.SaveTransactions(mapper.Map<ImportTransactionsDTO>(viewModel));
        if (result)
        {
            return Ok();
        }

        return BadRequest();
    }
    
    
}