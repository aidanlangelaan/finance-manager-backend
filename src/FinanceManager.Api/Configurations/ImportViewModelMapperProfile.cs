using AutoMapper;
using FinanceManager.Api.Models.Import;
using FinanceManager.Business.Services.Import;

namespace FinanceManager.Api.Configurations;

public class ImportViewModelMapperProfile : Profile
{
    public ImportViewModelMapperProfile()
    {
        CreateViewModelMapping();
    }

    private void CreateViewModelMapping()
    {
        // ViewModel --> DTO 
        CreateMap<ImportTransactionsViewModel, ImportTransactionsDTO>();
        
        // DTO --> ViewModel
        
    }
}