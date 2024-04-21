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
                    { new Guid("52d74348-dde1-4353-bd58-706db15236a7"), "329daada-29b3-4f27-99c6-3e03bc85d58e", "User", "USER" },
                    { new Guid("54202c0d-daf0-44a3-98b0-70180722261a"), "54202c0d-daf0-44a3-98b0-70180722261a", "SuperAdmin", "SUPERADMIN" },
                    { new Guid("58a771f2-82d5-4b0f-be01-089757c42fb8"), "6d22a17a-76f9-4a33-9e63-88ed1e2ce5e2", "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("0a5bfb58-d88a-4c47-9253-3e65a6a96fa6"), 0, "9c9e4cdf-06ea-4b63-8575-3d0f65439542", "nikitinroma2605@gmail.com", true, "Roman", "Nikitin", false, null, "NIKITINROMA2605@GMAIL.COM", "NIKITINROMA2605@GMAIL.COM", "AQAAAAEAACcQAAAAEKG+AbhX+FclnqGZzRQjpf4Fgho5cJ/CEi3EtBYGHT1hmv92MQJYNyJagGNFl0rb3A==", null, false, null, "6f6bb449-b685-4565-ae90-6bfe3cc7dbe0", false, "nikitinroma2605@gmail.com" });

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
                keyValue: new Guid("52d74348-dde1-4353-bd58-706db15236a7"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("58a771f2-82d5-4b0f-be01-089757c42fb8"));

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
