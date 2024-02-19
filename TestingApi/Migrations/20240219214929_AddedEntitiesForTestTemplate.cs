using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedEntitiesForTestTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameRestriction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestDifficultyRestriction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectRestriction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationRestriction = table.Column<int>(type: "int", nullable: true),
                    CreatedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionsPoolTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameRestriction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumOfQuestionsToBeGeneratedRestriction = table.Column<int>(type: "int", nullable: true),
                    GenerationStrategyRestriction = table.Column<int>(type: "int", nullable: true),
                    CreatedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsPoolTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsPoolTemplates_TestTemplates_TestTemplateId",
                        column: x => x.TestTemplateId,
                        principalTable: "TestTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionsPoolTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextRestriction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxScoreRestriction = table.Column<int>(type: "int", nullable: true),
                    CreatedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionTemplates_QuestionsPoolTemplates_QuestionsPoolTemplateId",
                        column: x => x.QuestionsPoolTemplateId,
                        principalTable: "QuestionsPoolTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionTemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TextRestriction = table.Column<int>(type: "int", nullable: true),
                    IsCorrectRestriction = table.Column<bool>(type: "bit", nullable: true),
                    CreatedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerTemplates_QuestionTemplates_QuestionTemplateId",
                        column: x => x.QuestionTemplateId,
                        principalTable: "QuestionTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerTemplates_QuestionTemplateId",
                table: "AnswerTemplates",
                column: "QuestionTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsPoolTemplates_TestTemplateId",
                table: "QuestionsPoolTemplates",
                column: "TestTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTemplates_QuestionsPoolTemplateId",
                table: "QuestionTemplates",
                column: "QuestionsPoolTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerTemplates");

            migrationBuilder.DropTable(
                name: "QuestionTemplates");

            migrationBuilder.DropTable(
                name: "QuestionsPoolTemplates");

            migrationBuilder.DropTable(
                name: "TestTemplates");
        }
    }
}
