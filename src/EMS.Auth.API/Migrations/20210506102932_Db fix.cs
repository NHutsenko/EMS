using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Auth.API.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Dbfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tokens_accessToken_refreshToken",
                schema: "auth",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "auth",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "auth",
                table: "Tokens",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "refreshToken",
                schema: "auth",
                table: "Tokens",
                type: "varchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "accessToken",
                schema: "auth",
                table: "Tokens",
                type: "varchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_accessToken",
                schema: "auth",
                table: "Tokens",
                column: "accessToken");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_refreshToken",
                schema: "auth",
                table: "Tokens",
                column: "refreshToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tokens_accessToken",
                schema: "auth",
                table: "Tokens");

            migrationBuilder.DropIndex(
                name: "IX_Tokens_refreshToken",
                schema: "auth",
                table: "Tokens");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "auth",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                schema: "auth",
                table: "Tokens",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "refreshToken",
                schema: "auth",
                table: "Tokens",
                type: "varchar(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "accessToken",
                schema: "auth",
                table: "Tokens",
                type: "varchar(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_accessToken_refreshToken",
                schema: "auth",
                table: "Tokens",
                columns: new[] { "accessToken", "refreshToken" });
        }
    }
}
