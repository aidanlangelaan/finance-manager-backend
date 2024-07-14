using AutoMapper;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data.Entities;

namespace FinanceManager.Business.configurations;

public class CategoryMapperProfile : Profile
{
    public CategoryMapperProfile()
    {
        CreateMap<Category, GetCategoryDTO>()
            .IncludeAllDerived();

        CreateMap<CreateCategoryDTO, Category>();

        CreateMap<UpdateCategoryDTO, Category>();
    }
}