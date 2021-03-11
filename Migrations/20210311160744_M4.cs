using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplzQuestionnaire.Migrations
{
    public partial class M4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TextAnswer",
                table: "UserAnswers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextAnswer",
                table: "UserAnswers");
        }
    }
}
