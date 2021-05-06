using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.Core.API.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.CreateTable(
                name: "Holidays",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    holidayDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    toDoDay = table.Column<DateTime>(type: "datetime", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotivationModificators",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    modValue = table.Column<double>(type: "float", nullable: false),
                    StaffId = table.Column<long>(type: "bigint", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivationModificators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "People",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    secondName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bornedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    contactType = table.Column<int>(type: "int", nullable: false),
                    personId = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_People_personId",
                        column: x => x.personId,
                        principalSchema: "core",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayOffs",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<int>(type: "int", nullable: false),
                    hours = table.Column<double>(type: "float", nullable: false),
                    isPaid = table.Column<bool>(type: "bit", nullable: false),
                    personId = table.Column<long>(type: "bigint", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayOffs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayOffs_People_personId",
                        column: x => x.personId,
                        principalSchema: "core",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OtherPayments",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<double>(type: "float", nullable: false),
                    personId = table.Column<long>(type: "bigint", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherPayments_People_personId",
                        column: x => x.personId,
                        principalSchema: "core",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonPhotos",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    base64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    personId = table.Column<long>(type: "bigint", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonPhotos_People_personId",
                        column: x => x.personId,
                        principalSchema: "core",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hourRate = table.Column<double>(type: "float", nullable: false),
                    teamId = table.Column<long>(type: "bigint", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Teams_teamId",
                        column: x => x.teamId,
                        principalSchema: "core",
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    personId = table.Column<long>(type: "bigint", nullable: true),
                    managerId = table.Column<long>(type: "bigint", nullable: false),
                    positionId = table.Column<long>(type: "bigint", nullable: false),
                    motivationModId = table.Column<long>(type: "bigint", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_MotivationModificators_motivationModId",
                        column: x => x.motivationModId,
                        principalSchema: "core",
                        principalTable: "MotivationModificators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_People_managerId",
                        column: x => x.managerId,
                        principalSchema: "core",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Staff_People_personId",
                        column: x => x.personId,
                        principalSchema: "core",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Staff_Positions_positionId",
                        column: x => x.positionId,
                        principalSchema: "core",
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_personId",
                schema: "core",
                table: "Contacts",
                column: "personId");

            migrationBuilder.CreateIndex(
                name: "IX_DayOffs_personId",
                schema: "core",
                table: "DayOffs",
                column: "personId");

            migrationBuilder.CreateIndex(
                name: "IX_OtherPayments_personId",
                schema: "core",
                table: "OtherPayments",
                column: "personId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonPhotos_personId",
                schema: "core",
                table: "PersonPhotos",
                column: "personId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_teamId",
                schema: "core",
                table: "Positions",
                column: "teamId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_managerId",
                schema: "core",
                table: "Staff",
                column: "managerId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_motivationModId",
                schema: "core",
                table: "Staff",
                column: "motivationModId",
                unique: true,
                filter: "[motivationModId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_personId",
                schema: "core",
                table: "Staff",
                column: "personId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_positionId",
                schema: "core",
                table: "Staff",
                column: "positionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts",
                schema: "core");

            migrationBuilder.DropTable(
                name: "DayOffs",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Holidays",
                schema: "core");

            migrationBuilder.DropTable(
                name: "OtherPayments",
                schema: "core");

            migrationBuilder.DropTable(
                name: "PersonPhotos",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Staff",
                schema: "core");

            migrationBuilder.DropTable(
                name: "MotivationModificators",
                schema: "core");

            migrationBuilder.DropTable(
                name: "People",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Positions",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Teams",
                schema: "core");
        }
    }
}
