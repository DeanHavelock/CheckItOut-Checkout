using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckItOut.Payments.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    MerchantId = table.Column<string>(nullable: false),
                    AccountNumber = table.Column<string>(nullable: true),
                    SortCode = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true),
                    Csv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.MerchantId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRequests",
                columns: table => new
                {
                    PaymentRequestId = table.Column<string>(nullable: false),
                    InvoiceId = table.Column<string>(nullable: true),
                    MerchantId = table.Column<string>(nullable: true),
                    Amount = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequests", x => x.PaymentRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<string>(nullable: false),
                    InvoiceId = table.Column<string>(nullable: true),
                    BankSimTransactionId = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    RecipientMerchantId = table.Column<string>(nullable: true),
                    SenderCardNumber = table.Column<string>(nullable: true),
                    CurrencyCode = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "MerchantId", "AccountNumber", "CardNumber", "Csv", "FullName", "SortCode" },
                values: new object[] { "TEST", "1111111111111111", "1234123412341234", "234", "bob", "111111" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "PaymentRequests");

            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
