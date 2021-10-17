using AutoMapper;
using FinanceManager.Business.Services.Models;
using FinanceManager.Data.Entities;

namespace FinanceManager.Business.configurations
{
    public class TransactionMapperProfile : Profile
    {
        public TransactionMapperProfile()
        {
            CreateMap<Transaction, GetTransactionDTO>()
                .IncludeAllDerived();

            CreateMap<CreateTransactionDTO, Transaction>();

            CreateMap<UpdateTransactionDTO, Transaction>();
        }
    }
}
