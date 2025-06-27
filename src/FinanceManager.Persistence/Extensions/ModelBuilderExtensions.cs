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
                    .HasPrincipalKey(nameof(User.KeycloakId))
                    .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity(clrType)
                    .HasOne(typeof(User), "UpdatedBy")
                    .WithMany()
                    .HasForeignKey("UpdatedById")
                    .HasPrincipalKey(nameof(User.KeycloakId))
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
}