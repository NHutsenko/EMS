using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Core.API.Migrations
{
    public partial class DbFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "Teams",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "Staff",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "Positions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "PersonPhotos",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "People",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "OtherPayments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "MotivationModificators",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "Holidays",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "DayOffs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "core",
                table: "Contacts",
                newName: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "Teams",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "Staff",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "Positions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "PersonPhotos",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "People",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "OtherPayments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "MotivationModificators",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "Holidays",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "DayOffs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "core",
                table: "Contacts",
                newName: "Id");
        }
    }
}
