using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
                    .Property("RowVersion")
                    .IsRowVersion()
                    .HasColumnType("xid");
            }

            // Configure auditable-entity properties
            if (auditableType.IsAssignableFrom(clrType))
            {
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
    }
}
