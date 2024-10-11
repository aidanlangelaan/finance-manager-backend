using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FinanceManager.Api.Identity;

public class EmailConfirmationTokenProviderOptions(IConfiguration configuration)
    : IConfigureNamedOptions<DataProtectionTokenProviderOptions>
{
    public void Configure(string? name, DataProtectionTokenProviderOptions options)
    {
        if (name == "EmailConfirmationTokenProviderOptions")
        {
            options.TokenLifespan = TimeSpan.FromHours(Convert.ToDouble(configuration["Authentication:EmailConfirmationTokenExpirationInHours"]));
        }
    }

    public void Configure(DataProtectionTokenProviderOptions options)
    {
    }
}