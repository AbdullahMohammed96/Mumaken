using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddNetOrderInFinancial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Total",
                table: "FinancialAccounts",
                newName: "TotalOrderPrice");

            migrationBuilder.AddColumn<double>(
                name: "ProviderPrice",
                table: "FinancialAccounts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderPrice",
                table: "FinancialAccounts");

            migrationBuilder.RenameColumn(
                name: "TotalOrderPrice",
                table: "FinancialAccounts",
                newName: "Total");
        }
    }
}
