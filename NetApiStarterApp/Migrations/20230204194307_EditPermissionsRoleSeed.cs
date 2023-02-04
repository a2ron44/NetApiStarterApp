using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetApiStarterApp.Migrations
{
    public partial class EditPermissionsRoleSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_permission");

            migrationBuilder.DropTable(
                name: "permission_type");

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission_role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_role", x => x.id);
                    table.ForeignKey(
                        name: "fk_permission_role_permission_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_permission_role_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "permission",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "DefaultAccess" },
                    { 2, "ViewData" },
                    { 3, "EditData" },
                    { 4, "CreateData" },
                    { 5, "DeleteData" }
                });

            migrationBuilder.InsertData(
                table: "permission_role",
                columns: new[] { "id", "permission_id", "role_id" },
                values: new object[,]
                {
                    { new Guid("479a7808-c99c-4c7c-b35e-5c297896f521"), 1, "bdbfb90f-49d1-4f2c-ae59-431af8a0dd6b" },
                    { new Guid("a9f5e9f0-ec78-46ff-99a3-a61dce02d835"), 1, "6d777ec1-44f0-4d0d-a411-fa5f4c7c7e81" },
                    { new Guid("c0a301ed-d1d9-4950-a155-8992cfcdb01c"), 1, "5e25621a-95f8-438a-a72e-360ddf98cc58" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_permission_role_permission_id",
                table: "permission_role",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "ix_permission_role_role_id",
                table: "permission_role",
                column: "role_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "permission_role");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.CreateTable(
                name: "permission_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permission_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_permission",
                columns: table => new
                {
                    permission_types_id = table.Column<int>(type: "integer", nullable: false),
                    roles_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permission", x => new { x.permission_types_id, x.roles_id });
                    table.ForeignKey(
                        name: "fk_role_permission_permission_type_permission_types_id",
                        column: x => x.permission_types_id,
                        principalTable: "permission_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_role_permission_role_roles_id",
                        column: x => x.roles_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "permission_type",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "ViewData" },
                    { 2, "EditData" },
                    { 3, "CreateData" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_role_permission_roles_id",
                table: "role_permission",
                column: "roles_id");
        }
    }
}
