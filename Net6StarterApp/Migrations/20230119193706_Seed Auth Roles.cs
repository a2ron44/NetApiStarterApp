using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net6StarterApp.Migrations
{
    public partial class SeedAuthRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f22df42-ae19-4cd6-a049-53967dbdf208", "734d9acc-9fa6-48e0-b84e-83f0a3a14ef7", "SuperAdmin", "SUPERADMIN" },
                    { "8d394b43-470e-41f1-ad84-4b0b4e7723df", "1779584e-c208-46bf-83dc-321bec269a6c", "User", "USER" },
                    { "a0138572-c3d2-4780-bb63-28c36bf53ff6", "46092073-a70e-4de6-a3e6-4f52486ff577", "Support", "SUPPORT" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f22df42-ae19-4cd6-a049-53967dbdf208");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d394b43-470e-41f1-ad84-4b0b4e7723df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0138572-c3d2-4780-bb63-28c36bf53ff6");
        }
    }
}
