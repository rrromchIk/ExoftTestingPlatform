using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class IntroducingTotalScoreAndUserScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Result",
                table: "UserTests",
                newName: "UserScore");

            migrationBuilder.AddColumn<float>(
                name: "TotalScore",
                table: "UserTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "UserTests");

            migrationBuilder.RenameColumn(
                name: "UserScore",
                table: "UserTests",
                newName: "Result");
        }
    }
}
