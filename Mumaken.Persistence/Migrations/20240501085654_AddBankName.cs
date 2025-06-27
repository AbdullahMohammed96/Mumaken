using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddBankName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BankAccount",
                table: "AdminBankAccounts",
                newName: "BankName");

            migrationBuilder.RenameColumn(
                name: "AccountAccount",
                table: "AdminBankAccounts",
                newName: "BankAccountName");

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "AdminBankAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "AdminBankAccounts");

            migrationBuilder.RenameColumn(
                name: "BankName",
                table: "AdminBankAccounts",
                newName: "BankAccount");

            migrationBuilder.RenameColumn(
                name: "BankAccountName",
                table: "AdminBankAccounts",
                newName: "AccountAccount");
        }
    }
}
