using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class addimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Img",
                table: "SplashScreens",
                newName: "ImgEn");

            migrationBuilder.AddColumn<string>(
                name: "ImgAr",
                table: "SplashScreens",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgAr",
                table: "SplashScreens");

            migrationBuilder.RenameColumn(
                name: "ImgEn",
                table: "SplashScreens",
                newName: "Img");
        }
    }
}
