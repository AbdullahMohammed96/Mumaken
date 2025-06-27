using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddNalitiyRelationInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_NationalityId",
                table: "Orders",
                column: "NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Nationalities_NationalityId",
                table: "Orders",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Nationalities_NationalityId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_NationalityId",
                table: "Orders");
        }
    }
}
