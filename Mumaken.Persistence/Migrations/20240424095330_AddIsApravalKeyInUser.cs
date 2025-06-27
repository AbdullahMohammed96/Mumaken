using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class AddIsApravalKeyInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.AddColumn<bool>(
                name: "IsAppravel",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAppravel",
                table: "AspNetUsers");

           
        }
    }
}
