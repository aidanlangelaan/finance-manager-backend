using FinanceManager.Application.ImportJobs.Dtos;
using FinanceManager.Application.Common.Models.Paging;

namespace FinanceManager.Application.ImportJobs.Interfaces;

public interface IImportJobService
{
    Task<ImportJobDetailsResponseDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<PagedResult<ImportJobResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct);

    Task<int> CreateAsync(CreateImportJobDto dto, Stream stream, CancellationToken ct);

    Task<bool> UpdateAsync(int id, UpdateImportJobDto dto, CancellationToken ct);
}
