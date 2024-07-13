using FinanceManager.Data.Entities;
using FinanceManager.Data.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FinanceManager.Data;

public class FinanceManagerDbContext(DbContextOptions<FinanceManagerDbContext> options)
    : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, UserToken>(options)
{
    public DbSet<Account> Accounts { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Import> Imports { get; set; }
    
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region Auditing and concurrency

        // TODO: not happy with this as it is...
        // setup auditable and concurrency properties
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var rowVersionProperty = entityType.FindProperty("RowVersion");
            if (rowVersionProperty != null)
            {
                rowVersionProperty.SetDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
                rowVersionProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
                rowVersionProperty.AddAnnotation("MySql:ValueGenerationStrategy",
                    MySqlValueGenerationStrategy.ComputedColumn);
            }

            var createdOnAtProperty = entityType.FindProperty("CreatedOnAt");
            if (createdOnAtProperty != null)
            {
                createdOnAtProperty.SetDefaultValueSql("CURRENT_TIMESTAMP");
                createdOnAtProperty.ValueGenerated = ValueGenerated.OnAdd;
                createdOnAtProperty.AddAnnotation("MySql:ValueGenerationStrategy",
                    MySqlValueGenerationStrategy.IdentityColumn);
            }

            var updatedOnAtProperty = entityType.FindProperty("UpdatedOnAt");
            if (updatedOnAtProperty != null)
            {
                updatedOnAtProperty.SetColumnType("datetime");
                updatedOnAtProperty.SetDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
                updatedOnAtProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
                updatedOnAtProperty.AddAnnotation("MySql:ValueGenerationStrategy",
                    MySqlValueGenerationStrategy.ComputedColumn);
            }
        }

        #endregion

        #region Authentication

        builder.Entity<User>(b =>
        {
            b.ToTable("Users");
            b.Property(u => u.LockoutEnd).HasColumnType("datetime");
        });

        builder.Entity<IdentityUserClaim<Guid>>(b => { b.ToTable("UserClaims"); });

        builder.Entity<IdentityUserLogin<Guid>>(b => { b.ToTable("UserLogins"); });

        builder.Entity<UserToken>(b =>
        {
            b.ToTable("UserTokens");
            b.Property(t => t.Value).IsRequired();
            b.Property(t => t.Value).HasColumnName("AccessToken");
        });

        builder.Entity<Role>(b => { b.ToTable("Roles"); });

        builder.Entity<IdentityRoleClaim<Guid>>(b => { b.ToTable("RoleClaims"); });

        builder.Entity<IdentityUserRole<Guid>>(b => { b.ToTable("UserRoles"); });

        #endregion Authentication

        builder.Entity<Transaction>()
            .HasOne(t => t.ToAccount)
            .WithMany(a => a.TransactionsTo)
            .HasForeignKey(t => t.ToAccountId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Transaction>()
            .HasOne(t => t.FromAccount)
            .WithMany(a => a.TransactionsFrom)
            .HasForeignKey(t => t.FromAccountId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Seed();
    }

    /// <summary>
    /// Asynchronously save the changes
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ChangeTracker.ApplyAuditInformation();
        return await base.SaveChangesAsync(cancellationToken);
    }
}