using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FinanceManager.Api.Identity;


public class PasswordResetTokenProviderOptions(IConfiguration configuration)
    : IConfigureNamedOptions<DataProtectionTokenProviderOptions>
{
    public void Configure(string? name, DataProtectionTokenProviderOptions options)
    {
        if (name == "PasswordResetTokenProvider")
        {
            options.TokenLifespan = TimeSpan.FromHours(Convert.ToDouble(configuration["Authentication:PasswordResetTokenExpirationInHours"]));
        }
    }

    public void Configure(DataProtectionTokenProviderOptions options)
    {
    }
}