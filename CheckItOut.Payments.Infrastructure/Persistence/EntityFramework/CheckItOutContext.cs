using CheckItOut.Payments.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CheckItOut.Payments.Infrastructure.Persistence.EntityFramework
{
    public class CheckItOutContext : DbContext
    {
        public CheckItOutContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Merchant> Merchants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Payment>().HasKey(e => e.PaymentId);
        }
    }

    public class CheckItOutContextFactory : IDesignTimeDbContextFactory<CheckItOutContext>
    {
        public CheckItOutContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CheckItOutContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CheckItOut;Integrated Security=true");

            return new CheckItOutContext(optionsBuilder.Options);
        }
    }
}
