using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class CreatedRelationsBetweenTmplsAndInstances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tests_TemplateId",
                table: "Tests",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsPools_TemplateId",
                table: "QuestionsPools",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TemplateId",
                table: "Questions",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_TemplateId",
                table: "Answers",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AnswerTemplates_TemplateId",
                table: "Answers",
                column: "TemplateId",
                principalTable: "AnswerTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionTemplates_TemplateId",
                table: "Questions",
                column: "TemplateId",
                principalTable: "QuestionTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionsPools_QuestionsPoolTemplates_TemplateId",
                table: "QuestionsPools",
                column: "TemplateId",
                principalTable: "QuestionsPoolTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestTemplates_TemplateId",
                table: "Tests",
                column: "TemplateId",
                principalTable: "TestTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AnswerTemplates_TemplateId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionTemplates_TemplateId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionsPools_QuestionsPoolTemplates_TemplateId",
                table: "QuestionsPools");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestTemplates_TemplateId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_TemplateId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_QuestionsPools_TemplateId",
                table: "QuestionsPools");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TemplateId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Answers_TemplateId",
                table: "Answers");
        }
    }
}
