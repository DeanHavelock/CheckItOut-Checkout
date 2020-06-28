using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckItOut.Payments.Infrastructure.Migrations
{
    public partial class PaymentEnitityUpdatesPropertiesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payments");
        }
    }
}
