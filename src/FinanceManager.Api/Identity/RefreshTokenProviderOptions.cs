using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FinanceManager.Api.Identity;

public class RefreshTokenProviderOptions(IConfiguration configuration)
    : IConfigureNamedOptions<DataProtectionTokenProviderOptions>
{
    public void Configure(string? name, DataProtectionTokenProviderOptions options)
    {
        if (name == "RefreshTokenProviderOptions")
        {
            options.TokenLifespan = TimeSpan.FromHours(Convert.ToDouble(configuration["Authentication:RefreshTokenExpirationInHours"]));
        }
    }

    public void Configure(DataProtectionTokenProviderOptions options)
    {
    }
}