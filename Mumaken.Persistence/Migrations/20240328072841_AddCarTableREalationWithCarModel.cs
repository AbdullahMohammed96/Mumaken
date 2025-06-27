using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddCarTableREalationWithCarModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarTypeId",
                table: "CarModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_CarTypeId",
                table: "CarModels",
                column: "CarTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarTypes_CarTypeId",
                table: "CarModels",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarTypes_CarTypeId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_CarTypeId",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "CarTypeId",
                table: "CarModels");
        }
    }
}
