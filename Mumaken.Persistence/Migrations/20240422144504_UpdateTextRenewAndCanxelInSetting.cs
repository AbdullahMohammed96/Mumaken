using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class UpdateTextRenewAndCanxelInSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextRenewAndCanxelEn",
                table: "Settings",
                newName: "TextRenewAndCancelEn");

            migrationBuilder.RenameColumn(
                name: "TextRenewAndCanxelAr",
                table: "Settings",
                newName: "TextRenewAndCancelAr");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextRenewAndCancelEn",
                table: "Settings",
                newName: "TextRenewAndCanxelEn");

            migrationBuilder.RenameColumn(
                name: "TextRenewAndCancelAr",
                table: "Settings",
                newName: "TextRenewAndCanxelAr");
        }
    }
}
