using FinanceManager.Application.Categories.Dtos;
using FinanceManager.Application.Common.Models.Paging;

namespace FinanceManager.Application.Categories.Interfaces;

public interface ICategoryService
{
    Task<CategoryResponseDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<PagedResult<CategoryResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct);

    Task<int> CreateAsync(CreateCategoryDto dto, CancellationToken ct);

    Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
