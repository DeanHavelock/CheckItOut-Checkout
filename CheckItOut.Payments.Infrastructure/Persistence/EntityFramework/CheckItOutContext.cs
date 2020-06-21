using CheckItOut.Payments.Domain;
using Microsoft.EntityFrameworkCore;


namespace CheckItOut.Payments.Infrastructure.Persistence.EntityFramework
{
    public class CheckItOutContext : DbContext
    {
        public CheckItOutContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Payment> Payments { get; set; }
    }
}
