using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Services.Models;

namespace FinanceManager.Api.Configurations;

public class CategoryViewModelMapperProfile : Profile
{
    public CategoryViewModelMapperProfile()
    {
        CreateViewModelMapping();
    }

    private void CreateViewModelMapping()
    {
        CreateMap<GetCategoryDTO, GetCategoryViewModel>();

        CreateMap<CreateCategoryViewModel, CreateCategoryDTO>();

        CreateMap<UpdateCategoryViewModel, UpdateCategoryDTO>();
    }
}