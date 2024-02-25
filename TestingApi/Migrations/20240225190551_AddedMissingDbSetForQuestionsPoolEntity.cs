using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissingDbSetForQuestionsPoolEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionsPool_QuestionsPoolId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsPool_Tests_TestId",
                table: "QuestionsPool");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionsPool",
                table: "QuestionsPool");

            migrationBuilder.RenameTable(
                name: "QuestionsPool",
                newName: "QuestionsPools");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionsPool_TestId",
                table: "QuestionsPools",
                newName: "IX_QuestionsPools_TestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionsPools",
                table: "QuestionsPools",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionsPools_QuestionsPoolId",
                table: "Questions",
                column: "QuestionsPoolId",
                principalTable: "QuestionsPools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsPools_Tests_TestId",
                table: "QuestionsPools",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionsPools_QuestionsPoolId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsPools_Tests_TestId",
                table: "QuestionsPools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionsPools",
                table: "QuestionsPools");

            migrationBuilder.RenameTable(
                name: "QuestionsPools",
                newName: "QuestionsPool");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionsPools_TestId",
                table: "QuestionsPool",
                newName: "IX_QuestionsPool_TestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionsPool",
                table: "QuestionsPool",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionsPool_QuestionsPoolId",
                table: "Questions",
                column: "QuestionsPoolId",
                principalTable: "QuestionsPool",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsPool_Tests_TestId",
                table: "QuestionsPool",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
