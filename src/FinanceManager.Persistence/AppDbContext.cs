using FinanceManager.Application.Interfaces;
using FinanceManager.Domain.Entities;
using FinanceManager.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService, TimeProvider timeProvider)
    : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all Entity Type Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.ConfigureCommonBaseEntities();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = currentUserService.KeycloakId;
        var now = timeProvider.GetUtcNow().UtcDateTime;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOnAt = now;
                entry.Entity.UpdatedOnAt = now;

                entry.Entity.CreatedById = userId;
                entry.Entity.UpdatedById = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedOnAt = now;
                entry.Entity.UpdatedById = userId;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
