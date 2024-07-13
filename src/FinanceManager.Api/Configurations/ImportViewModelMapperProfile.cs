using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Services.Models;

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
        CreateMap<GetImportDTO, GetImportViewModel>();
    }
}