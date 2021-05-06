using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Auth.API.Migrations
{
    public partial class TokenLengths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tokens_accessToken_refreshToken",
                schema: "auth",
                table: "Tokens");

            migrationBuilder.AlterColumn<string>(
                name: "refreshToken",
                schema: "auth",
                table: "Tokens",
                type: "varchar(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "accessToken",
                schema: "auth",
                table: "Tokens",
                type: "varchar(256)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_accessToken_refreshToken",
                schema: "auth",
                table: "Tokens",
                columns: new[] { "accessToken", "refreshToken" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tokens_accessToken_refreshToken",
                schema: "auth",
                table: "Tokens");

            migrationBuilder.AlterColumn<string>(
                name: "refreshToken",
                schema: "auth",
                table: "Tokens",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "accessToken",
                schema: "auth",
                table: "Tokens",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_accessToken_refreshToken",
                schema: "auth",
                table: "Tokens",
                columns: new[] { "accessToken", "refreshToken" },
                unique: true,
                filter: "[accessToken] IS NOT NULL AND [refreshToken] IS NOT NULL");
        }
    }
}
