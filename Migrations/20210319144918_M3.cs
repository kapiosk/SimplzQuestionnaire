using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplzQuestionnaire.Migrations
{
    public partial class M3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Answers_AnswerId1",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_AnswerId1",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "AnswerId1",
                table: "Answers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerId1",
                table: "Answers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_AnswerId1",
                table: "Answers",
                column: "AnswerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Answers_AnswerId1",
                table: "Answers",
                column: "AnswerId1",
                principalTable: "Answers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
