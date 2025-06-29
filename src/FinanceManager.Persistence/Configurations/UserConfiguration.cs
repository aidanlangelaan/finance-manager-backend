using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configure properties
        builder.Property(u => u.KeycloakId)
            .IsRequired()
            .HasColumnType("uuid");

        builder.Property(u => u.DisplayName)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasColumnType("varchar(100)");

        // Configure relationships
        builder.HasOne(t => t.CreatedBy)
            .WithMany(u => u.Users)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
