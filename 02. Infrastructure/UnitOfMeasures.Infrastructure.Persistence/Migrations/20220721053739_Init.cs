using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnitOfMeasures.Infrastructure.Persistence.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dimensions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimensions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PersianName = table.Column<string>(type: "TEXT", nullable: false),
                    Abbreviation = table.Column<string>(type: "TEXT", nullable: false),
                    ConversionFactor = table.Column<double>(type: "REAL", nullable: false),
                    FormulaToBase = table.Column<string>(type: "TEXT", nullable: false),
                    FormulaFromBase = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    DimensionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_Dimensions_DimensionId",
                        column: x => x.DimensionId,
                        principalTable: "Dimensions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Units_DimensionId",
                table: "Units",
                column: "DimensionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Dimensions");
        }
    }
}
