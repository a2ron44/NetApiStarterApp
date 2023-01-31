using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net6StarterApp.Migrations
{
    public partial class SeedPermissions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "CreateData" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
