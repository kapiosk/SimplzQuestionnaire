using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplzQuestionnaire.Migrations
{
    public partial class M1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AcceptsCustomAnswer",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptsCustomAnswer",
                table: "Questions");
        }
    }
}
