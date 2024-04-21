using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameBaseEntityProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Users",
                newName: "ModifiedTimestamp");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Users",
                newName: "CreatedTimestamp");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Tests",
                newName: "ModifiedTimestamp");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Tests",
                newName: "CreatedTimestamp");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "QuestionsPool",
                newName: "ModifiedTimestamp");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "QuestionsPool",
                newName: "CreatedTimestamp");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Questions",
                newName: "ModifiedTimestamp");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Questions",
                newName: "CreatedTimestamp");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Answers",
                newName: "ModifiedTimestamp");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Answers",
                newName: "CreatedTimestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedTimestamp",
                table: "Users",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "CreatedTimestamp",
                table: "Users",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "ModifiedTimestamp",
                table: "Tests",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "CreatedTimestamp",
                table: "Tests",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "ModifiedTimestamp",
                table: "QuestionsPool",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "CreatedTimestamp",
                table: "QuestionsPool",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "ModifiedTimestamp",
                table: "Questions",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "CreatedTimestamp",
                table: "Questions",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "ModifiedTimestamp",
                table: "Answers",
                newName: "Modified");

            migrationBuilder.RenameColumn(
                name: "CreatedTimestamp",
                table: "Answers",
                newName: "Created");
        }
    }
}
