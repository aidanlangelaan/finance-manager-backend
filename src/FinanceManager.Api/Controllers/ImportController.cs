using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceManager.Business.Interfaces;
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
        /// <response code="200">TODO</response>
        /// <response code="400">TODO</response>          
        [HttpPost("transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportTransactions(ICollection<IFormFile> files)
        {
            await _importService.ProcessImport(files);

            return Ok();
        }
    }
}
