using FinanceManager.Application.Tags.Dtos;
using FinanceManager.Application.Common.Models.Paging;

namespace FinanceManager.Application.Tags.Interfaces;

public interface ITagService
{
    Task<TagResponseDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<PagedResult<TagResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct);

    Task<int> CreateAsync(CreateTagDto dto, CancellationToken ct);

    Task<bool> UpdateAsync(int id, UpdateTagDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
