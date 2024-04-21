using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultTextPropForQuestAndAnswTmpl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultText",
                table: "QuestionTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DefaultText",
                table: "AnswerTemplates",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultText",
                table: "QuestionTemplates");

            migrationBuilder.DropColumn(
                name: "DefaultText",
                table: "AnswerTemplates");
        }
    }
}
