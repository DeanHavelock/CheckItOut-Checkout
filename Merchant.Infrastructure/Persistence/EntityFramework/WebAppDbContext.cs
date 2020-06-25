using Merchant.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Merchant.Infrastructure.Persistence.EntityFramework
{
    public class WebAppDbContext : DbContext
    {
        public WebAppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().Ignore(e=>e.OrderItems);
        }
    }

    public class WebAppDbContextFactory : IDesignTimeDbContextFactory<WebAppDbContext>
    {
        public WebAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebAppDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DevMerchantApp;Integrated Security=true");

            return new WebAppDbContext(optionsBuilder.Options); 
        }
    }
}
