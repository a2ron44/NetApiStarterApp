using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Net6StarterApp.Migrations
{
    public partial class RenamePermissionRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions");

            migrationBuilder.RenameTable(
                name: "RolePermissions",
                newName: "PermissionTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PermissionTypes",
                table: "PermissionTypes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PermissionTypes",
                table: "PermissionTypes");

            migrationBuilder.RenameTable(
                name: "PermissionTypes",
                newName: "RolePermissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolePermissions",
                table: "RolePermissions",
                column: "Id");
        }
    }
}
