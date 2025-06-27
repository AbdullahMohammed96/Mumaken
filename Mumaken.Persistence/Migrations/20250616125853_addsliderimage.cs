using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class addsliderimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Sliders",
                newName: "ImageEn");

            migrationBuilder.AddColumn<string>(
                name: "ImageAr",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageAr",
                table: "Sliders");

            migrationBuilder.RenameColumn(
                name: "ImageEn",
                table: "Sliders",
                newName: "Image");
        }
    }
}
