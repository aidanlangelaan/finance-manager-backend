using FinanceManager.Application.Tags.Dtos;
using FinanceManager.Application.Tags.Interfaces;
using FinanceManager.Application.Tags.Mapping;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Common.Interfaces.Paging;
using FinanceManager.Application.Common.Models.Paging;

namespace FinanceManager.Application.Tags.Services;

public class TagService(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser,
    IPagingService pagingService,
    TagMapper mapper) : ITagService
{
    public async Task<TagResponseDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var tag = await unitOfWork.Tags.GetByIdAsync(id, currentUser.UserId, ct);
        return tag is null ? null : mapper.ToDto(tag);
    }

    public async Task<PagedResult<TagResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct)
    {
        var query = unitOfWork.Tags.GetAllAsync(currentUser.UserId);
        return await pagingService.ToPagedResultAsync(query, paging, mapper.ToDto, ct);
    }

    public async Task<int> CreateAsync(CreateTagDto dto, CancellationToken ct)
    {
        var entity = mapper.ToEntity(dto);
        await unitOfWork.Tags.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateTagDto dto, CancellationToken ct)
    {
        var tag = await unitOfWork.Tags.FindEntityByIdAsync(id, currentUser.UserId, ct);
        if (tag is null)
            return false;

        mapper.UpdateEntity(dto, tag);

        await unitOfWork.Tags.UpdateAsync(tag, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var tag = await unitOfWork.Tags.FindEntityByIdAsync(id, currentUser.UserId, ct);
        if (tag is null)
            return false;

        await unitOfWork.Tags.DeleteAsync(tag, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}

