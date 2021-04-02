using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplzQuestionnaire.Migrations
{
    public partial class M5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionGroups",
                columns: table => new
                {
                    SessionGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuestionnaireId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionGroups", x => x.SessionGroupId);
                });

            migrationBuilder.CreateTable(
                name: "SessionGroupUsers",
                columns: table => new
                {
                    SessionGroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionGroupUsers", x => new { x.UserId, x.SessionGroupId });
                    table.ForeignKey(
                        name: "FK_SessionGroupUsers_SessionGroups_SessionGroupId",
                        column: x => x.SessionGroupId,
                        principalTable: "SessionGroups",
                        principalColumn: "SessionGroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionGroupUsers_SessionGroupId",
                table: "SessionGroupUsers",
                column: "SessionGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionGroupUsers_UserId_SessionGroupId",
                table: "SessionGroupUsers",
                columns: new[] { "UserId", "SessionGroupId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionGroupUsers");

            migrationBuilder.DropTable(
                name: "SessionGroups");
        }
    }
}
