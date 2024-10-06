using FinanceManager.Data.Entities;
using FinanceManager.Data.Enums;
using FinanceManager.Data.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            var newCategory = new Category
            {
                Id = (int)category,
                Name = category.StringValue(),
                Description = string.Empty
            };
            newCategory.Hash = HashUtils.GetHash(newCategory);

            categories.Add(newCategory);
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

    public static void ConfigureBaseEntity<TBaseEntity>(this ModelBuilder modelBuilder,
        Action<EntityTypeBuilder<TBaseEntity>> configure)
        where TBaseEntity : class
    {
        // Configure the base entity
        var baseEntityBuilder = modelBuilder.Entity<TBaseEntity>();
        configure(baseEntityBuilder);

        // Set default values for specific properties
        SetDefaultValuesForProperties(baseEntityBuilder);

        // Recursively configure derived entities
        ConfigureDerivedEntities(modelBuilder, typeof(TBaseEntity));
    }

    private static void ConfigureDerivedEntities(ModelBuilder modelBuilder, Type baseEntityType)
    {
        // Find all derived entities
        var derivedTypes = baseEntityType.Assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && baseEntityType.IsAssignableFrom(t) &&
                        t != baseEntityType);

        foreach (var derivedType in derivedTypes)
        {
            // Get the generic method for configuring the derived entity
            var method = typeof(ModelBuilder).GetMethod(nameof(ModelBuilder.Entity), new Type[] { })?
                .MakeGenericMethod(derivedType);
            var entityTypeBuilder = method?.Invoke(modelBuilder, null) as EntityTypeBuilder;

            // Set the discriminator value
            entityTypeBuilder?.HasDiscriminator<string>("Discriminator")
                .HasValue(derivedType, derivedType.Name);

            // Set default values for specific properties
            SetDefaultValuesForProperties(entityTypeBuilder);

            // Recursively configure the derived entity
            ConfigureDerivedEntities(modelBuilder, derivedType);
        }
    }

    private static void SetDefaultValuesForProperties(EntityTypeBuilder? entityTypeBuilder)
    {
        if (entityTypeBuilder == null)
        {
            return;
        }
        
        var rowVersionProperty = entityTypeBuilder.Metadata.ClrType.GetProperty("RowVersion");
        if (rowVersionProperty != null && rowVersionProperty.PropertyType == typeof(DateTime))
        {
            var property = entityTypeBuilder.Property("RowVersion");
            property.HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            property.HasColumnType("datetime");
            property.ValueGeneratedOnUpdate();
            property.HasAnnotation("MySql:ValueGenerationStrategy",
                MySqlValueGenerationStrategy.ComputedColumn);
        }
        
        // Check if the entity type has a property named "CreatedOnAt"
        var createdOnAtProperty = entityTypeBuilder.Metadata.ClrType.GetProperty("CreatedOnAt");
        if (createdOnAtProperty != null && createdOnAtProperty.PropertyType == typeof(DateTime))
        {
            // Set the default value SQL for "CreatedOnAt"
            var property = entityTypeBuilder.Property("CreatedOnAt");
            property.HasDefaultValueSql("CURRENT_TIMESTAMP");
            property.HasColumnType("datetime");
            property.ValueGeneratedOnAdd();
            property.HasAnnotation("MySql:ValueGenerationStrategy",
                MySqlValueGenerationStrategy.ComputedColumn);
        }
        
        var updatedOnAtProperty = entityTypeBuilder.Metadata.ClrType.GetProperty("UpdatedOnAt");
        if (updatedOnAtProperty != null && updatedOnAtProperty.PropertyType == typeof(DateTime))
        {
            // Set the default value SQL for "UpdatedOnAt"
            var property = entityTypeBuilder.Property("UpdatedOnAt");
            property.HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
            property.HasColumnType("datetime");
            property.ValueGeneratedOnAddOrUpdate();
            property.HasAnnotation("MySql:ValueGenerationStrategy",
                MySqlValueGenerationStrategy.ComputedColumn);
        }
    }
}