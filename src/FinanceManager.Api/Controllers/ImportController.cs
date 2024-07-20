using FinanceManager.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data.Enums;

namespace FinanceManager.Api.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ImportController(IImportService importService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets all imports
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/import
    ///
    /// </remarks>
    /// <returns>List of imports</returns>
    /// <response code="200">Imports have been retrieved</response>
    /// <response code="400">Failed to process request</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetImportViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        var imports = await importService.GetAll();
        return Ok(imports);
    }

    /// <summary>
    /// Gets a single import by its id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/import/1
    ///
    /// </remarks>
    /// <returns>A single import</returns>
    /// <response code="200">Import for given id has been retrieved</response>
    /// <response code="404">Import not found for given id</response>
    /// <response code="400">Failed to process request</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetImportViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(int id)
    {
        var import = await importService.GetById(id);
        if (import != null)
        {
            return Ok(import);
        }

        return NotFound();
    }
    
    /// <summary>
    /// Imports a single csv file containing transactions
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/import
    ///
    /// </remarks>
    /// <response code="200">File has been imported and is ready to be processed</response>
    /// <response code="400">Failed to import file</response>          
    [HttpPost]
    [ProducesResponseType(typeof(List<CsvImportResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ImportTransactions(IFormFile file, BankType bankType, bool assignCategories = false)
    {
        var viewModel = new ImportTransactionsViewModel
        {
            File = file,
            Bank = bankType,
            AssignCategories = assignCategories
        };
        
        var result = await importService.SaveImportFile(mapper.Map<ImportTransactionsDTO>(viewModel));
        if (result)
        {
            return Ok();
        }

        return BadRequest();
    }
}