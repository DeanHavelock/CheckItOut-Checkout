using Microsoft.EntityFrameworkCore.Migrations;

namespace Merchant.Infrastructure.Migrations
{
    public partial class Order_PaymentIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentId",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Orders");
        }
    }
}
