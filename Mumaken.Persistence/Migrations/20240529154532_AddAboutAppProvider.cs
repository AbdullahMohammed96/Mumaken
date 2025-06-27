using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddAboutAppProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboutAppProviderAr",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AboutAppProviderEn",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AboutAppProviderAr",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "AboutAppProviderEn",
                table: "Settings");
        }
    }
}
