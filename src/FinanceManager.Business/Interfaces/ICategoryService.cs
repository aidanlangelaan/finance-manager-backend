using FinanceManager.Business.Services.Models;

namespace FinanceManager.Business.Interfaces;

public interface ICategoryService
{
    public Task<List<GetCategoryDTO>> GetAll();

    public Task<GetCategoryDTO?> GetById(int id);

    public Task<GetCategoryDTO> Create(CreateCategoryDTO model);

    public Task Update(UpdateCategoryDTO model);

    public Task Delete(int id);
}