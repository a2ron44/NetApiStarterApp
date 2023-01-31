using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Net6StarterApp.Authentication.Permissions;

namespace Net6StarterApp.Authentication.Data.SeedData
{
	public class SeedDataRoles : IEntityTypeConfiguration<IdentityRole>
	{

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            //need to add guid or these get recreated every migration.
            //Add roles here.
            builder.HasData(
                new IdentityRole
                {
                    Id = "5e25621a-95f8-438a-a72e-360ddf98cc58",
                    Name = RoleType.NormalUser,
                    NormalizedName = RoleType.NormalUser.ToUpper()
                },
                new IdentityRole
                {
                    Id = "6d777ec1-44f0-4d0d-a411-fa5f4c7c7e81",
                    Name = RoleType.SuperAdmin,
                    NormalizedName = RoleType.SuperAdmin.ToUpper()
                },
                new IdentityRole
                {
                    Id = "bdbfb90f-49d1-4f2c-ae59-431af8a0dd6b",
                    Name = RoleType.Support,
                    NormalizedName = RoleType.Support.ToUpper()
                }
            ); ;
        }
    }

    public class SeedPermissions : IEntityTypeConfiguration<PermissionType>
    {

        public void Configure(EntityTypeBuilder<PermissionType> builder)
        {

            var permissionList = PermissionType.GetAllPermissions();

            builder.HasData(permissionList);
        }
    }
}

