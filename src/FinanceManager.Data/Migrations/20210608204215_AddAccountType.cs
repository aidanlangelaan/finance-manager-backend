using Microsoft.EntityFrameworkCore.Migrations;

namespace FinanceManager.Data.Migrations
{
    public partial class AddAccountType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceAfter",
                table: "Transactions");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "Accounts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Accounts");

            migrationBuilder.AddColumn<double>(
                name: "BalanceAfter",
                table: "Transactions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
