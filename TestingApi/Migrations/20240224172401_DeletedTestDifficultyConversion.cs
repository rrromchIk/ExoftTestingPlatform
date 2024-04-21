using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class DeletedTestDifficultyConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Difficulty",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue:0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Difficulty",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
