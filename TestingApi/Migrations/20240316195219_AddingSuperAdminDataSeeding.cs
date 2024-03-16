using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddingSuperAdminDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedBy", "CreatedTimestamp", "Email", "EmailConfirmed", "FirstName", "LastName", "ModifiedBy", "ModifiedTimestamp", "ProfilePictureFilePath", "UserRole" },
                values: new object[] { new Guid("0a5bfb58-d88a-4c47-9253-3e65a6a96fa6"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "nikitinroma2605@gmail.com", true, "Roman", "Nikitin", null, null, null, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0a5bfb58-d88a-4c47-9253-3e65a6a96fa6"));
        }
    }
}
