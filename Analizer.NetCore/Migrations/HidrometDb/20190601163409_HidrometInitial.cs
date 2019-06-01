using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Analizer.NetCore.Migrations.HidrometDb
{
    public partial class HidrometInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hidromet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: true, defaultValueSql: "(getdate())"),
                    City = table.Column<string>(unicode: false, nullable: false),
                    Temperature = table.Column<double>(nullable: false),
                    DewPoint = table.Column<double>(nullable: false),
                    Precipitation = table.Column<int>(nullable: false),
                    Wind = table.Column<int>(nullable: false),
                    MeterologicalPressure = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hidromet", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hidromet");
        }
    }
}
