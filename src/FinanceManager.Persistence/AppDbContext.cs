using System.Linq.Expressions;
using FinanceManager.Application.Interfaces;
using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService)
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
        
        ConfigureAuditableEntity<Account>(modelBuilder);
        ConfigureAuditableEntity<Transaction>(modelBuilder);
        ConfigureAuditableEntity<Category>(modelBuilder);
        ConfigureAuditableEntity<Tag>(modelBuilder);
        ConfigureAuditableEntity<User>(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = currentUserService.KeycloakId;
        var now = DateTime.UtcNow;
        
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
    
    private static void ConfigureAuditableEntity<TEntity>(ModelBuilder modelBuilder)
        where TEntity : AuditableEntity
    {
        modelBuilder.Entity<TEntity>()
            .HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey(e => e.CreatedById)
            .HasPrincipalKey(u => u.KeycloakId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TEntity>()
            .HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey(e => e.UpdatedById)
            .HasPrincipalKey(u => u.KeycloakId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}