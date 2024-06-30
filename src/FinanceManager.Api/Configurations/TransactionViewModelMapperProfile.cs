using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Services.Models;

namespace FinanceManager.Api.Configurations;

public class TransactionViewModelMapperProfile : Profile
{
    public TransactionViewModelMapperProfile()
    {
        CreateViewModelMapping();
    }

    private void CreateViewModelMapping()
    {
        CreateMap<GetTransactionDTO, GetTransactionViewModel>();

        CreateMap<CreateTransactionViewModel, CreateTransactionDTO>();

        CreateMap<UpdateTransactionViewModel, UpdateTransactionDTO>();
    }
}