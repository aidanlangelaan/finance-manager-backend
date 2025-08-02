using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FinanceManager.Persistence.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureCommonBaseEntities(this ModelBuilder modelBuilder)
    {
        var baseType = typeof(EntityBase);
        var auditableType = typeof(AuditableEntity);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            // Configure entity-base properties
            if (baseType.IsAssignableFrom(clrType))
            {
                modelBuilder.Entity(clrType)
                    .Property("Id")
                    .ValueGeneratedOnAdd();

                var rowVersionProperty = modelBuilder.Entity(clrType)
                    .Property("RowVersion")
                    .IsConcurrencyToken()
                    .HasColumnName("xmin")
                    .HasColumnType("xid")
                    .ValueGeneratedOnAddOrUpdate();

                rowVersionProperty.Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
                rowVersionProperty.Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            }

            if (!auditableType.IsAssignableFrom(clrType)) continue;

            // Configure auditable-entity properties
            modelBuilder.Entity(clrType)
                .Property("CreatedById")
                .HasColumnType("int");

            modelBuilder.Entity(clrType)
                .Property("UpdatedById")
                .HasColumnType("int");

            modelBuilder.Entity(clrType)
                .Property("CreatedOnAt")
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity(clrType)
                .Property("UpdatedOnAt")
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            modelBuilder.Entity(clrType)
                .HasOne(typeof(User), "CreatedBy")
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity(clrType)
                .HasOne(typeof(User), "UpdatedBy")
                .WithMany()
                .HasForeignKey("UpdatedById")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public static void ConfigureUserRelationships(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Accounts)
            .WithOne(a => a.CreatedBy)
            .HasForeignKey(a => a.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Transactions)
            .WithOne(t => t.CreatedBy)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Categories)
            .WithOne(c => c.CreatedBy)
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Tags)
            .WithOne(t => t.CreatedBy)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Users)
            .WithOne(t => t.CreatedBy)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
