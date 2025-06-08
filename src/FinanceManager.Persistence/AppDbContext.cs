using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence;

public class AppDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
        
    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }
    
    public DbSet<Account> Accounts;
    public DbSet<Transaction> Transactions;
    public DbSet<UserProfile> UserProfiles;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;
        var now = DateTime.UtcNow;
        
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedOnAt = now;
                entry.Entity.UpdatedOnAt = now;
                
                entry.Entity.CreatedBy = userId;
                entry.Entity.UpdatedBy = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedOnAt = now;
                entry.Entity.UpdatedBy = userId;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}