using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCore.Data.Migrations
{
    public partial class SeedIndentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 8, 30, 9, 53, 10, 614, DateTimeKind.Local).AddTicks(1667));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"), "7aea8ac9-7520-4df0-b1e6-e0ff6d4fd53e", "Adminstrator Role ", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"), new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812") });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"), 0, "4c96cd6b-83bc-4812-be1f-47cd796179d9", new DateTime(1999, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "dinhson14399@gmail.com", true, "Dinh", "Son", false, null, "dinhson14399@gmail.com", "admin", "AQAAAAEAACcQAAAAEOc4HKZ8raHK0fKzDiv2Xubq9FaXBuoLs6TAmvwnRmPqKnISORE4FMpCFgRdTIRBvw==", null, false, "", false, "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"));

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("0d5b7850-46c1-4c80-99c4-d94fc38a3ea7"), new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b38060f2-8b1c-47ae-80aa-2cf1b518b812"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                columns: new[] { "RoleId", "UserId" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 8, 30, 9, 14, 2, 789, DateTimeKind.Local).AddTicks(5949));
        }
    }
}
