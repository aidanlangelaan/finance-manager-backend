using System.Linq.Expressions;
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
    public DbSet<User> Users;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        
        modelBuilder.Entity<Account>()
            .HasMany(a => a.SourceTransactions)
            .WithOne(t => t.SourceAccount)
            .HasForeignKey(t => t.SourceAccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Account>()
            .HasMany(a => a.DestinationTransactions)
            .WithOne(t => t.DestinationAccount)
            .HasForeignKey(t => t.DestinationAccountId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.Subcategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Transaction>()
            .HasMany(t => t.Tags)
            .WithMany(t => t.Transactions)
            .UsingEntity(j => j.ToTable("TransactionTags"));
        
        ConfigureAuditableEntity(modelBuilder, t => t.CreatedBy, u => u.Transactions);
        ConfigureAuditableEntity(modelBuilder, a => a.CreatedBy, u => u.Accounts);
        ConfigureAuditableEntity(modelBuilder, c => c.CreatedBy, u => u.Categories);
        ConfigureAuditableEntity(modelBuilder, t => t.CreatedBy, u => u.Tags);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;
        var now = DateTime.UtcNow;
        
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            // if (entry.State == EntityState.Added)
            // {
            //     entry.Entity.CreatedOnAt = now;
            //     entry.Entity.UpdatedOnAt = now;
            //     
            //     entry.Entity.CreatedBy = userId;
            //     entry.Entity.UpdatedBy = userId;
            // }
            // else if (entry.State == EntityState.Modified)
            // {
            //     entry.Entity.UpdatedOnAt = now;
            //     entry.Entity.UpdatedBy = userId;
            // }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
    
    private static void ConfigureAuditableEntity<TEntity>(
        ModelBuilder modelBuilder,
        Expression<Func<TEntity, User?>> navigation,
        Expression<Func<User, IEnumerable<TEntity>?>> inverse)
        where TEntity : AuditableEntity
    {
        modelBuilder.Entity<TEntity>()
            .HasOne(navigation)
            .WithMany(inverse)
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}