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
        // ViewModel --> DTO 
        CreateMap<RegisterUserViewModel, RegisterUserDTO>();
        CreateMap<ResendEmailConfirmationViewModel, ResendEmailConfirmationDTO>();
        CreateMap<ConfirmEmailAddressViewModel, ConfirmEmailAddressDTO>();
        CreateMap<ForgotPasswordViewModel, ForgotPasswordDTO>();
        CreateMap<ResetPasswordViewModel, ResetPasswordDTO>();
        CreateMap<ResetPasswordViewModel, ResetPasswordDTO>();
        CreateMap<LoginUserViewModel, LoginUserDTO>();
        CreateMap<RefreshAccessTokenViewModel, RefreshAccessTokenDTO>();
        
        // DTO --> ViewModel
        CreateMap<AuthorizationTokenDTO, AuthorizationTokenViewModel>();
    }
}