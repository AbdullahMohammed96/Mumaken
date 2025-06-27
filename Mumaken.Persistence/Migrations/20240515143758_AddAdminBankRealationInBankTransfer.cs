using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddAdminBankRealationInBankTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminBankId",
                table: "BankTransfers",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_BankTransfers_AdminBankId",
                table: "BankTransfers",
                column: "AdminBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransfers_AdminBankAccounts_AdminBankId",
                table: "BankTransfers",
                column: "AdminBankId",
                principalTable: "AdminBankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransfers_AdminBankAccounts_AdminBankId",
                table: "BankTransfers");

            migrationBuilder.DropIndex(
                name: "IX_BankTransfers_AdminBankId",
                table: "BankTransfers");

            migrationBuilder.DropColumn(
                name: "AdminBankId",
                table: "BankTransfers");
        }
    }
}
