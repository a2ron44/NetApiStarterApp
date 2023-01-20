using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net6StarterApp.Migrations
{
    public partial class SeedAuthRoles5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
