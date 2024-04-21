using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedTestTemplateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameRestriction",
                table: "TestTemplates");

            migrationBuilder.DropColumn(
                name: "SubjectRestriction",
                table: "TestTemplates");

            migrationBuilder.DropColumn(
                name: "TextRestriction",
                table: "QuestionTemplates");

            migrationBuilder.DropColumn(
                name: "TextRestriction",
                table: "AnswerTemplates");

            migrationBuilder.RenameColumn(
                name: "TestDifficultyRestriction",
                table: "TestTemplates",
                newName: "DefaultSubject");

            migrationBuilder.RenameColumn(
                name: "DurationRestriction",
                table: "TestTemplates",
                newName: "DefaultTestDifficulty");

            migrationBuilder.RenameColumn(
                name: "NameRestriction",
                table: "QuestionsPoolTemplates",
                newName: "DefaultName");

            migrationBuilder.AddColumn<int>(
                name: "DefaultDuration",
                table: "TestTemplates",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCorrectRestriction",
                table: "AnswerTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultDuration",
                table: "TestTemplates");

            migrationBuilder.RenameColumn(
                name: "DefaultTestDifficulty",
                table: "TestTemplates",
                newName: "DurationRestriction");

            migrationBuilder.RenameColumn(
                name: "DefaultSubject",
                table: "TestTemplates",
                newName: "TestDifficultyRestriction");

            migrationBuilder.RenameColumn(
                name: "DefaultName",
                table: "QuestionsPoolTemplates",
                newName: "NameRestriction");

            migrationBuilder.AddColumn<string>(
                name: "NameRestriction",
                table: "TestTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectRestriction",
                table: "TestTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextRestriction",
                table: "QuestionTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCorrectRestriction",
                table: "AnswerTemplates",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "TextRestriction",
                table: "AnswerTemplates",
                type: "int",
                nullable: true);
        }
    }
}
