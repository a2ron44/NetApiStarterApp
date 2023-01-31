using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Net6StarterApp.Migrations
{
    public partial class SeedPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22f4ab86-737b-499d-a63a-2af3d8936951");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "267f2e74-1936-4b28-8dfe-d4cdee0cc7c1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72a03123-8a8f-44ea-8a1e-15bfc3ef0f9c");

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5e25621a-95f8-438a-a72e-360ddf98cc58", null, "User", "USER" },
                    { "6d777ec1-44f0-4d0d-a411-fa5f4c7c7e81", null, "SuperAdmin", "SUPERADMIN" },
                    { "bdbfb90f-49d1-4f2c-ae59-431af8a0dd6b", null, "Support", "SUPPORT" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ViewData" },
                    { 2, "EditData" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22f4ab86-737b-499d-a63a-2af3d8936951", "8324ba2c-e53c-4007-9dfd-795c1183b085", "User", "USER" },
                    { "267f2e74-1936-4b28-8dfe-d4cdee0cc7c1", "60650c7a-a57b-490b-9f7e-2fa4617944c4", "SuperAdmin", "SUPERADMIN" },
                    { "72a03123-8a8f-44ea-8a1e-15bfc3ef0f9c", "fd787422-15c0-49c5-a55c-f1ff684a17f7", "Support", "SUPPORT" }
                });
        }
    }
}
