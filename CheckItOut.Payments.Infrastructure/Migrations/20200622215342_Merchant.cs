using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckItOut.Payments.Infrastructure.Migrations
{
    public partial class Merchant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");
        }
    }
}
