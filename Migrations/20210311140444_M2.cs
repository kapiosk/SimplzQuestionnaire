using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplzQuestionnaire.Migrations
{
    public partial class M2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveQuestionId",
                table: "Questionnaires",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveQuestionId",
                table: "Questionnaires");
        }
    }
}
