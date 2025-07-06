using FinanceManager.Application.Categories.Dtos;
using FinanceManager.Application.Categories.Interfaces;
using FinanceManager.Application.Categories.Mapping;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Application.Common.Interfaces.Paging;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Common.Models.Paging;

namespace FinanceManager.Application.Categories.Services;

public class CategoryService(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser,
    IPagingService pagingService,
    CategoryMapper mapper) : ICategoryService
{
    public async Task<CategoryResponseDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var category = await unitOfWork.Categories.GetByIdAsync(id, currentUser.UserId, ct);
        return category is null ? null : mapper.ToDto(category);
    }

    public async Task<PagedResult<CategoryResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct)
    {
        var query = unitOfWork.Categories.GetAllAsync(currentUser.UserId);
        return await pagingService.ToPagedResultAsync(query, paging, mapper.ToDto, ct);
    }

    public async Task<int> CreateAsync(CreateCategoryDto dto, CancellationToken ct)
    {
        var entity = mapper.ToEntity(dto);
        await unitOfWork.Categories.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken ct)
    {
        var category = await unitOfWork.Categories.FindEntityByIdAsync(id, currentUser.UserId, ct);
        if (category is null)
            return false;

        mapper.UpdateEntity(dto, category);

        await unitOfWork.Categories.UpdateAsync(category, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var category = await unitOfWork.Categories.FindEntityByIdAsync(id, currentUser.UserId, ct);
        if (category is null)
            return false;

        await unitOfWork.Categories.DeleteAsync(category, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}

