using BadBrokerTest.Models;
using System.Data.Entity;

namespace BadBrokerTest.DataContext
{
    public class TestDataContext : DbContext
    {
        public TestDataContext() : base("DbConnectionString") {
            Database.SetInitializer(new DatabaseInitializer());
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class {
            return base.Set<TEntity>();
        }

        public DbSet<Currency> Currencies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Currency>().HasKey(e => new { e.Date, e.SellCurrency, e.BuyCurrency });
        }
    }
}