using System;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetApiStarterApp.NetApiStarterLibrary.Permissions;
using NetApiStarterLibrary.Models;
using NetApiStarterLibrary.Permissions;

namespace NetApiStarterLibrary.Data.SeedData
{
	public class SeedDataRoles : IEntityTypeConfiguration<ApiRole>
	{

        public void Configure(EntityTypeBuilder<ApiRole> builder)
        {
            //need to add guid or these get recreated every migration.
            //Add roles here.
            builder.HasData(
                new ApiRole
                {
                    Id = "5e25621a-95f8-438a-a72e-360ddf98cc58",
                    Name = RoleType.NormalUser,
                    NormalizedName = RoleType.NormalUser.ToUpper()
                },
                new ApiRole
                {
                    Id = "6d777ec1-44f0-4d0d-a411-fa5f4c7c7e81",
                    Name = RoleType.SuperAdmin,
                    NormalizedName = RoleType.SuperAdmin.ToUpper()
                },
                new ApiRole
                {
                    Id = "bdbfb90f-49d1-4f2c-ae59-431af8a0dd6b",
                    Name = RoleType.Support,
                    NormalizedName = RoleType.Support.ToUpper()
                }
            ); ;
        }
    }

    public class SeedDataPermissions : IEntityTypeConfiguration<Permission>
    {

        public void Configure(EntityTypeBuilder<Permission> builder)
        {

            var permissionList = Permission.GetAllPermissions();

            builder.HasData(permissionList);
        }
    }


    public class SeedDataPermissionRoles : IEntityTypeConfiguration<PermissionRole>
    {

        public void Configure(EntityTypeBuilder<PermissionRole> builder)
        {

            builder.HasData(
                new PermissionRole {
                    Id = new Guid("c0a301ed-d1d9-4950-a155-8992cfcdb01c"),
                    PermissionId = PermissionType.DefaultAccess,
                    RoleId = "5e25621a-95f8-438a-a72e-360ddf98cc58",
                },
                new PermissionRole
                {
                    Id = new Guid("a9f5e9f0-ec78-46ff-99a3-a61dce02d835"),
                    PermissionId = PermissionType.DefaultAccess,
                    RoleId = "6d777ec1-44f0-4d0d-a411-fa5f4c7c7e81",
                },
                new PermissionRole
                {
                    Id = new Guid("479a7808-c99c-4c7c-b35e-5c297896f521"),
                    PermissionId = PermissionType.DefaultAccess,
                    RoleId = "bdbfb90f-49d1-4f2c-ae59-431af8a0dd6b",
                }
            );
                 
        }
    }

}

