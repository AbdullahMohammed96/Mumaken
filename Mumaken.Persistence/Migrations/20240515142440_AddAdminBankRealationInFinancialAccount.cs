using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddAdminBankRealationInFinancialAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminBankId",
                table: "FinancialAccounts",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccounts_AdminBankId",
                table: "FinancialAccounts",
                column: "AdminBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccounts_AdminBankAccounts_AdminBankId",
                table: "FinancialAccounts",
                column: "AdminBankId",
                principalTable: "AdminBankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccounts_AdminBankAccounts_AdminBankId",
                table: "FinancialAccounts");

            migrationBuilder.DropIndex(
                name: "IX_FinancialAccounts_AdminBankId",
                table: "FinancialAccounts");

            migrationBuilder.DropColumn(
                name: "AdminBankId",
                table: "FinancialAccounts");
        }
    }
}
