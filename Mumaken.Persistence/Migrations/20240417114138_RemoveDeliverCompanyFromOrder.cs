using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class RemoveDeliverCompanyFromOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_DeliveryCompanyId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_RenewCompanyId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliveryCompanyId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryCompanyId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "RenewCompanyId",
                table: "Orders",
                newName: "RentalCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_RenewCompanyId",
                table: "Orders",
                newName: "IX_Orders_RentalCompanyId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationDbUserId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationDbUserId",
                table: "Orders",
                column: "ApplicationDbUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationDbUserId",
                table: "Orders",
                column: "ApplicationDbUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_RentalCompanyId",
                table: "Orders",
                column: "RentalCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_ApplicationDbUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_RentalCompanyId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ApplicationDbUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ApplicationDbUserId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "RentalCompanyId",
                table: "Orders",
                newName: "RenewCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_RentalCompanyId",
                table: "Orders",
                newName: "IX_Orders_RenewCompanyId");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryCompanyId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryCompanyId",
                table: "Orders",
                column: "DeliveryCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_DeliveryCompanyId",
                table: "Orders",
                column: "DeliveryCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_RenewCompanyId",
                table: "Orders",
                column: "RenewCompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
