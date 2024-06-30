using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Import;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImportController(IImportService importService) : ControllerBase
{
    /// <summary>
    /// Imports csv files containing transactions
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/import/transactions
    ///
    /// </remarks>
    /// <returns>Results for the imported transactions</returns>
    /// <response code="200">File(s) have successfully been processed and imported</response>
    /// <response code="400">Failed to process files and import records</response>          
    [HttpPost("transactions")]
    [ProducesResponseType(typeof(List<CsvImportResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<CsvImportResult>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportTransactions(ICollection<IFormFile> files)
    {
        List<CsvImportResult> result = [];
        if (files.Count <= 0) return BadRequest(result);

        result = await importService.ProcessImport(files);
        if (result.All(r => r.IsSuccess))
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}