using System;
using System.Collections.Generic;
using FinanceManager.Data.Entities;
using FinanceManager.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        SeedCategories(modelBuilder);
        
        SeedRoles(modelBuilder);
    }

    private static void SeedCategories(ModelBuilder modelBuilder)
    {
        List<Category> categories = [];
        foreach (Categories category in Enum.GetValues(typeof(Categories)))
        {
            categories.Add(new Category
            {
                Id = (int) category,
                Description = category.StringValue()
            });
        }

        modelBuilder.Entity<Category>().HasData(categories);
    }

    private static void SeedRoles(ModelBuilder modelBuilder)
    {
        List<Role> userRoles = [];
        foreach (Roles role in Enum.GetValues(typeof(Roles)))
        {
            userRoles.Add(new Role(role.StringValue())
            {
                Id = Guid.NewGuid(),
                NormalizedName = role.StringValue().ToUpper()
            });
        }
        
        modelBuilder.Entity<Role>().HasData(userRoles);
    }
}