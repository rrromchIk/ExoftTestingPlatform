using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class MakeQuestionsPoolNameUniqueWithinTheTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuestionsPools_Name",
                table: "QuestionsPools");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsPools_Name_TestId",
                table: "QuestionsPools",
                columns: new[] { "Name", "TestId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QuestionsPools_Name_TestId",
                table: "QuestionsPools");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsPools_Name",
                table: "QuestionsPools",
                column: "Name",
                unique: true);
        }
    }
}
