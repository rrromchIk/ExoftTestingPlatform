using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedAnsweringTimestampToUserAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AnsweringTime",
                table: "UserAnswers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnsweringTime",
                table: "UserAnswers");
        }
    }
}
