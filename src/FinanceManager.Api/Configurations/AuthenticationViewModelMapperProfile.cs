using AutoMapper;
using FinanceManager.Api.Models;
using FinanceManager.Business.Services.Models;

namespace FinanceManager.Api.Configurations;

public class AuthenticationViewModelMapperProfile : Profile
{
    public AuthenticationViewModelMapperProfile()
    {
        CreateViewModelMapping();
    }

    private void CreateViewModelMapping()
    {
        CreateMap<LoginUserViewModel, LoginUserDTO>();

        CreateMap<RegisterUserViewModel, RegisterUserDTO>();
    }
}