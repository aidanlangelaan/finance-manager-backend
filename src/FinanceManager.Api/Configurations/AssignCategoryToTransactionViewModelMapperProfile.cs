using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Services.Models;

namespace FinanceManager.Api.Configurations;

public class AssignCategoryToTransactionViewModelMapperProfile : Profile
{
    public AssignCategoryToTransactionViewModelMapperProfile()
    {
        CreateViewModelMapping();
    }

    private void CreateViewModelMapping()
    {
        CreateMap<AssignCategoryToTransactionViewModel, AssignCategoryToTransactionDTO>();
    }
}