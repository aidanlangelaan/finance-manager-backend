using FinanceManager.Application.ImportJobs.Dtos;
using FinanceManager.Application.ImportJobs.Interfaces;
using FinanceManager.Application.ImportJobs.Mapping;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Common.Interfaces.Paging;
using FinanceManager.Application.Common.Models.Paging;

namespace FinanceManager.Application.ImportJobs.Services;

public class ImportJobService(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser,
    IPagingService pagingService,
    ImportJobMapper mapper) : IImportJobService
{
    public async Task<ImportJobDetailsResponseDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var importJob = await unitOfWork.ImportJobs.GetByIdAsync(id, currentUser.UserId, ct);
        return importJob is null ? null : mapper.ToDetailsDto(importJob);
    }

    public async Task<PagedResult<ImportJobResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct)
    {
        var query = unitOfWork.ImportJobs.GetAllAsync(currentUser.UserId);
        return await pagingService.ToPagedResultAsync(query, paging, mapper.ToDto, ct);
    }

    public async Task<int> CreateAsync(CreateImportJobDto dto, Stream stream, CancellationToken ct)
    {
        var entity = mapper.ToEntity(dto);
        await unitOfWork.ImportJobs.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateImportJobDto dto, CancellationToken ct)
    {
        var importJob = await unitOfWork.ImportJobs.FindEntityByIdAsync(id, currentUser.UserId, ct);
        if (importJob is null)
            return false;

        mapper.UpdateEntity(dto, importJob);

        await unitOfWork.ImportJobs.UpdateAsync(importJob, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var importJob = await unitOfWork.ImportJobs.FindEntityByIdAsync(id, currentUser.UserId, ct);
        if (importJob is null)
            return false;

        await unitOfWork.ImportJobs.DeleteAsync(importJob, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}

