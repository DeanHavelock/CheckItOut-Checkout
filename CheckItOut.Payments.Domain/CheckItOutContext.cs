using Microsoft.EntityFrameworkCore;


namespace CheckItOut.Payments.Domain
{
    public class CheckItOutContext : DbContext
    {
        public CheckItOutContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Payment> Payments { get; set; }
    }
}
