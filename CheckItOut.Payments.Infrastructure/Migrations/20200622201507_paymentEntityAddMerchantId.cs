using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckItOut.Payments.Infrastructure.Migrations
{
    public partial class paymentEntityAddMerchantId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MerchantId",
                table: "Payments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "Payments");
        }
    }
}
