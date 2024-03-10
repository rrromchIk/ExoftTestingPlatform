using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Security.Migrations
{
    /// <inheritdoc />
    public partial class AddingSuperAdminDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("14544cd2-746b-4492-bfe0-2dbdad78f2d3"), "62ac21aa-282b-48a3-b5ff-b60919d58831", "Admin", "ADMIN" },
                    { new Guid("54202c0d-daf0-44a3-98b0-70180722261a"), "54202c0d-daf0-44a3-98b0-70180722261a", "SuperAdmin", "SUPERADMIN" },
                    { new Guid("883c74ad-69fe-4916-b16f-82534c7b8377"), "3cce5bf2-9bd6-487f-bb17-0cc8b6345bcb", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("0a5bfb58-d88a-4c47-9253-3e65a6a96fa6"), 0, "a5f34392-b061-4e4b-aa91-cc422a5761ec", "nikitinroma2605@gmail.com", true, "Roman", "Nikitin", false, null, null, "NIKITINROMA2605@GMAIL.COM", "AQAAAAEAACcQAAAAEFxL5HjwTT3xK2RcOxUIgEYRPqMqgkJCiseRlyOWuLcfJe1nbwBMquhHr0lZWo4lhA==", null, false, null, null, false, "nikitinroma2605@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("54202c0d-daf0-44a3-98b0-70180722261a"), new Guid("0a5bfb58-d88a-4c47-9253-3e65a6a96fa6") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("14544cd2-746b-4492-bfe0-2dbdad78f2d3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("883c74ad-69fe-4916-b16f-82534c7b8377"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("54202c0d-daf0-44a3-98b0-70180722261a"), new Guid("0a5bfb58-d88a-4c47-9253-3e65a6a96fa6") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("54202c0d-daf0-44a3-98b0-70180722261a"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("0a5bfb58-d88a-4c47-9253-3e65a6a96fa6"));

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
    }
}
