using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Security.Migrations
{
    /// <inheritdoc />
    public partial class AddedRefreshTokenPropForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("36b1e99e-150b-4125-b289-683c67e8cdbc"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7c628e3f-e74a-47a7-86f1-8cafd588b1fb"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("bab57d67-920c-40b3-9cb2-e70e281f128e"));

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("1e9e6c55-9071-4a83-8e79-f0d9abcbb3ee"), "5ba0b0ab-41ae-43ec-9393-0d1e4a8713da", "User", "USER" },
                    { new Guid("6fbb2f18-3b28-486d-80f9-3441cb2980c5"), "84a91d7d-f7ae-4a6f-99d8-55a1642382d2", "SuperAdmin", "SUPERADMIN" },
                    { new Guid("a77c3769-2191-42d4-90c3-065f7582fd63"), "6f27606d-abc6-45dd-97b6-9228917e551e", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("1e9e6c55-9071-4a83-8e79-f0d9abcbb3ee"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6fbb2f18-3b28-486d-80f9-3441cb2980c5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a77c3769-2191-42d4-90c3-065f7582fd63"));

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("36b1e99e-150b-4125-b289-683c67e8cdbc"), "bdba5f86-db84-4b39-a27c-3e42f17adff8", "Admin", "ADMIN" },
                    { new Guid("7c628e3f-e74a-47a7-86f1-8cafd588b1fb"), "e5ed2574-6bbd-4ccd-a01d-1eff7d57109c", "User", "USER" },
                    { new Guid("bab57d67-920c-40b3-9cb2-e70e281f128e"), "f0e6bf8d-40d6-49e6-a1c7-8d4a734cd4e8", "SuperAdmin", "SUPERADMIN" }
                });
        }
    }
}
