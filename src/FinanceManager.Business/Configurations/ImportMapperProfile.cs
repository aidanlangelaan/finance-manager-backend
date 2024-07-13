using AutoMapper;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data.Entities;

namespace FinanceManager.Business.configurations;

public class ImportMapperProfile : Profile
{
    public ImportMapperProfile()
    {
        CreateMap<Import, GetImportDTO>()
            .IncludeAllDerived();
        
        CreateMap<CsvImportRabo, Transaction>()
            .ReverseMap();
    }
}