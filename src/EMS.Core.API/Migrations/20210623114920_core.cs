using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Core.API.Migrations
{
    public partial class core : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "roadMapId",
                schema: "core",
                table: "Staff",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "RoadMaps",
                schema: "core",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<int>(type: "int", nullable: false),
                    tasks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    staffId = table.Column<long>(type: "bigint", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadMaps", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_roadMapId",
                schema: "core",
                table: "Staff",
                column: "roadMapId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_RoadMaps_roadMapId",
                schema: "core",
                table: "Staff",
                column: "roadMapId",
                principalSchema: "core",
                principalTable: "RoadMaps",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_RoadMaps_roadMapId",
                schema: "core",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "RoadMaps",
                schema: "core");

            migrationBuilder.DropIndex(
                name: "IX_Staff_roadMapId",
                schema: "core",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "roadMapId",
                schema: "core",
                table: "Staff");
        }
    }
}
