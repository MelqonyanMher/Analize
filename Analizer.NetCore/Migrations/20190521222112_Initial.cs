using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Analizer.NetCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Itams",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CityId = table.Column<Guid>(nullable: false),
                    Day = table.Column<DateTime>(nullable: false),
                    Temperature = table.Column<double>(nullable: false),
                    DewPoint = table.Column<double>(nullable: false),
                    CompIndicatorDay = table.Column<double>(nullable: false),
                    Precipitation = table.Column<int>(nullable: false, defaultValue: 0),
                    CompIndicator = table.Column<double>(nullable: false),
                    ClassOfFireRisk = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Itams_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Itams_CityId",
                table: "Itams",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Itams");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
