﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CheckItOut.Payments.Infrastructure.Persistence.EntityFramework
{
    public class CheckItOutContext : DbContext
    {
        public CheckItOutContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Domain.Payment> Payments { get; set; }
        public DbSet<Domain.Merchant> Merchants { get; set; }
        public DbSet<Domain.PaymentRequest> PaymentRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CheckItOut.Payments.Domain.Payment>().HasKey(e => e.PaymentId);
            modelBuilder.Entity<Domain.Merchant>().HasData(new Domain.Merchant { MerchantId="TEST", FullName="bob", AccountNumber = "1111111111111111", SortCode = "111111", CardNumber= "1234123412341234", Csv="234" });
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
