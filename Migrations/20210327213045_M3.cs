using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplzQuestionnaire.Migrations
{
    public partial class M3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptsCustomAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IntegerCustomAnswer",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "CustomAnswer",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomAnswer",
                table: "Questions");

            migrationBuilder.AddColumn<bool>(
                name: "AcceptsCustomAnswer",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IntegerCustomAnswer",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
