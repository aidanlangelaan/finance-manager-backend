using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Import;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            _importService = importService;
        }

        /// <summary>
        /// Imports csv files containing transactions
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/import/transactions
        ///
        /// </remarks>
        /// <returns>TODO</returns>
        /// <response code="200">File(s) have successfully been processed and imported</response>
        /// <response code="400">Failed to process files and import records</response>          
        [HttpPost("transactions")]
        [ProducesResponseType(typeof(List<CsvImportResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<CsvImportResult>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportTransactions(ICollection<IFormFile> files)
        {
            List<CsvImportResult> result = new List<CsvImportResult>();
            if (files.Count > 0)
            {
                result = await _importService.ProcessImport(files);
                if (!result.Any(r => !r.IsSuccess))
                {
                    return Ok(result);
                }
            }

            return BadRequest(result);
        }
    }
}
