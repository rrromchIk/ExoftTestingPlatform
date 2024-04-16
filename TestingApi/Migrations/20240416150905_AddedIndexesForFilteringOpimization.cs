using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexesForFilteringOpimization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserTests_StartingTime",
                table: "UserTests",
                column: "StartingTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserTests_UserScore",
                table: "UserTests",
                column: "UserScore");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedTimestamp",
                table: "Users",
                column: "CreatedTimestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirstName",
                table: "Users",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastName",
                table: "Users",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifiedTimestamp",
                table: "Users",
                column: "ModifiedTimestamp");

            migrationBuilder.CreateIndex(
                name: "IX_TestTemplates_CreatedTimestamp",
                table: "TestTemplates",
                column: "CreatedTimestamp");

            migrationBuilder.CreateIndex(
                name: "IX_TestTemplates_DefaultDuration",
                table: "TestTemplates",
                column: "DefaultDuration");

            migrationBuilder.CreateIndex(
                name: "IX_TestTemplates_ModifiedTimestamp",
                table: "TestTemplates",
                column: "ModifiedTimestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_CreatedTimestamp",
                table: "Tests",
                column: "CreatedTimestamp");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_Duration",
                table: "Tests",
                column: "Duration");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_ModifiedTimestamp",
                table: "Tests",
                column: "ModifiedTimestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTests_StartingTime",
                table: "UserTests");

            migrationBuilder.DropIndex(
                name: "IX_UserTests_UserScore",
                table: "UserTests");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatedTimestamp",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FirstName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LastName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ModifiedTimestamp",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TestTemplates_CreatedTimestamp",
                table: "TestTemplates");

            migrationBuilder.DropIndex(
                name: "IX_TestTemplates_DefaultDuration",
                table: "TestTemplates");

            migrationBuilder.DropIndex(
                name: "IX_TestTemplates_ModifiedTimestamp",
                table: "TestTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Tests_CreatedTimestamp",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_Duration",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_ModifiedTimestamp",
                table: "Tests");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
