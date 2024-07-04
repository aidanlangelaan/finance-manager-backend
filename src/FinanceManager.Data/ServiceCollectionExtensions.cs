using System;
using FinanceManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDataServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FinanceManagerDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34))));

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<FinanceManagerDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}