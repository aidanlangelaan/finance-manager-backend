using AutoMapper;
using FinanceManager.Business.Services.Import;
using FinanceManager.Data.Entities;

namespace FinanceManager.Business.configurations
{
    public class ImportMapperProfile : Profile
    {
        public ImportMapperProfile()
        {
            CreateMap<CsvImportRabo, Transaction>()
                .ReverseMap();
        }
    }
}
