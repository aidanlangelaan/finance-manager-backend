using AutoMapper;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data;
using FinanceManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Business.Services;

public class CategoryService(FinanceManagerDbContext context, IMapper mapper) : ICategoryService
{
    public async Task<List<GetCategoryDTO>> GetAll()
    {
        var categories = await context.Categories.ToListAsync();
        return mapper.Map<List<GetCategoryDTO>>(categories);
    }

    public async Task<GetCategoryDTO?> GetById(int id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(t => t.Id == id);
        return mapper.Map<GetCategoryDTO>(category);
    }

    public async Task<GetCategoryDTO> Create(CreateCategoryDTO model)
    {
        var category = mapper.Map<Category>(model);
        var addCategory = await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        return mapper.Map<GetCategoryDTO>(addCategory.Entity);
    }

    public async Task Update(UpdateCategoryDTO model)
    {
        var category = await context.Categories.FirstOrDefaultAsync(t => t.Id == model.Id);
        if (category != null)
        {
            category = mapper.Map(model, category);
            context.Categories.Update(category);
            await context.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(t => t.Id == id);
        if (category != null)
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
        }
    }
}