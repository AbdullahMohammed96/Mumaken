using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class BankAdminIsNullInBankTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransfers_AdminBankAccounts_AdminBankId",
                table: "BankTransfers");

            migrationBuilder.AlterColumn<int>(
                name: "AdminBankId",
                table: "BankTransfers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");


       

            migrationBuilder.AddForeignKey(
                name: "FK_BankTransfers_AdminBankAccounts_AdminBankId",
                table: "BankTransfers",
                column: "AdminBankId",
                principalTable: "AdminBankAccounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankTransfers_AdminBankAccounts_AdminBankId",
                table: "BankTransfers");

            migrationBuilder.AlterColumn<int>(
                name: "AdminBankId",
                table: "BankTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

           
            migrationBuilder.AddForeignKey(
                name: "FK_BankTransfers_AdminBankAccounts_AdminBankId",
                table: "BankTransfers",
                column: "AdminBankId",
                principalTable: "AdminBankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
