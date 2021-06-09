using System.Threading;
using System.Threading.Tasks;
using FinanceManager.Data.Entities;
using FinanceManager.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Data
{
    public class FinanceManagerDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Transaction> Transactions { get; set; }


        public FinanceManagerDbContext(DbContextOptions<FinanceManagerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.ToAccount)
                .WithMany(a => a.TransactionsTo)
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.FromAccount)
                .WithMany(a => a.TransactionsFrom)
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        /// <summary>
		/// Asynchroniously save the changes
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.ApplyAuditInformation();

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
