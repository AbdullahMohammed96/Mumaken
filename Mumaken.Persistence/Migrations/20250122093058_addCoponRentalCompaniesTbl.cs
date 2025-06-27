using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mumaken.Persistence.Migrations
{
    public partial class addCoponRentalCompaniesTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoponRentalCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoponId = table.Column<int>(type: "int", nullable: false),
                    RentalCompanyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoponRentalCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoponRentalCompanies_AspNetUsers_RentalCompanyId",
                        column: x => x.RentalCompanyId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoponRentalCompanies_Copon_CoponId",
                        column: x => x.CoponId,
                        principalTable: "Copon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoponRentalCompanies_CoponId",
                table: "CoponRentalCompanies",
                column: "CoponId");

            migrationBuilder.CreateIndex(
                name: "IX_CoponRentalCompanies_RentalCompanyId",
                table: "CoponRentalCompanies",
                column: "RentalCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoponRentalCompanies");
        }
    }
}
