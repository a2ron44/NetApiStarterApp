using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net6StarterApp.Migrations
{
    public partial class SeedAuthRoles3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5e25621a-95f8-438a-a72e-360ddf98cc58", "9dc29b8a-10d5-4158-af5f-1c0f5cbc67ea", "SuperAdmin", "SUPERADMIN" },
                    { "6d777ec1-44f0-4d0d-a411-fa5f4c7c7e81", "5158f1d6-601c-40e5-a508-7521a496bc8b", "Support", "SUPPORT" },
                    { "bdbfb90f-49d1-4f2c-ae59-431af8a0dd6b", "7cd73d03-05eb-4c50-b86a-aa6781a4743a", "User", "USER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e25621a-95f8-438a-a72e-360ddf98cc58");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d777ec1-44f0-4d0d-a411-fa5f4c7c7e81");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bdbfb90f-49d1-4f2c-ae59-431af8a0dd6b");
        }
    }
}
