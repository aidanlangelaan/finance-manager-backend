using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        // Configure properties
        builder.Property(t => t.Name)
            .IsRequired()
            .HasColumnType("varchar(100)");

        // Configure relationships
        builder.HasOne(t => t.CreatedBy)
            .WithMany(u => u.Tags)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
