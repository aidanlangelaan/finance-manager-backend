using AutoMapper;
using FinanceManager.Business.Services.Import;
using FinanceManager.Data.Entities;

namespace FinanceManager.Business.configurations
{
    public class TransactionMapperProfile : Profile
    {
        public TransactionMapperProfile()
        {
            CreateMap<CsvImportRabo, Transaction>()
                .ReverseMap();
        }
    }
}
