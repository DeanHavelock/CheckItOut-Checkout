﻿// <auto-generated />
using CheckItOut.Payments.Infrastructure.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CheckItOut.Payments.Infrastructure.Migrations
{
    [DbContext(typeof(CheckItOutContext))]
    partial class CheckItOutContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CheckItOut.Payments.Domain.Merchant", b =>
                {
                    b.Property<string>("MerchantId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Csv")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SortCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MerchantId");

                    b.ToTable("Merchants");
                });

            modelBuilder.Entity("CheckItOut.Payments.Domain.Payment", b =>
                {
                    b.Property<string>("PaymentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("BankSimTransactionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrencyCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InvoiceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipientMerchantId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderCardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentId");

                    b.ToTable("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
