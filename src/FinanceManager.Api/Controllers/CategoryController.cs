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
public class CategoryController(ICategoryService categoryService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets all categories
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/category
    ///
    /// </remarks>
    /// <returns>List of categories</returns>
    /// <response code="200">Categories have been retrieved</response>
    /// <response code="400">Failed to process request</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetCategoryViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        var categories = await categoryService.GetAll();
        return Ok(categories);
    }

    /// <summary>
    /// Gets a single category by its id
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/category/1
    ///
    /// </remarks>
    /// <returns>A single category</returns>
    /// <response code="200">Category for given id has been retrieved</response>
    /// <response code="404">Category not found for given id</response>
    /// <response code="400">Failed to process request</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetCategoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(int id)
    {
        var category = await categoryService.GetById(id);
        if (category != null)
        {
            return Ok(category);
        }

        return NotFound();
    }

    /// <summary>
    /// Creates a category
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/category/1
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
    /// <returns>A category</returns>
    /// <response code="201">Category has been created</response>
    /// <response code="400">Request is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(GetCategoryViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateCategoryViewModel model)
    {
        var category = await categoryService.Create(mapper.Map<CreateCategoryDTO>(model));
        return Created($"api/category/{category.Id}", category);
    }

    /// <summary>
    /// Updates a category
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/category
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
    /// <response code="200">Category has been updated</response>
    /// <response code="400">Request is invalid</response>
    [HttpPut]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(UpdateCategoryViewModel model)
    {
        await categoryService.Update(mapper.Map<UpdateCategoryDTO>(model));
        return Ok();
    }

    /// <summary>
    /// Deletes a category
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/category/1
    ///
    /// </remarks>
    /// <response code="200">Category with given id has been deleted</response>
    /// <response code="404">Category not found for given id</response>
    /// <response code="400">Request is invalid</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await categoryService.GetById(id);
        if (category == null) return NotFound();
        await categoryService.Delete(category.Id);
        return Ok();
    }
}