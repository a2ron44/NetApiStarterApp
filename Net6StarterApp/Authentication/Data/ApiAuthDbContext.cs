﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Net6StarterApp.Authentication.Data.SeedData;
using Net6StarterApp.Authentication.Models;
using Net6StarterApp.Authentication.Permissions;
using Net6StarterApp.Data;

namespace Net6StarterApp.Authentication.Data
{
	public class ApiAuthDbContext : IdentityDbContext<ApiUser, ApiRole, string>
    {
        public ApiAuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed data for auth
            modelBuilder.ApplyConfiguration(new SeedDataRoles());
            modelBuilder.ApplyConfiguration(new SeedDataPermissions());

            modelBuilder.Entity<ApiUser>().ToTable("user");
            modelBuilder.Entity<ApiRole>().ToTable("role");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("user_role");
            modelBuilder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("user_claim"); });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("user_login"); });
            modelBuilder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("user_token"); });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("role_claim"); });

            modelBuilder.Entity<ApiRole>()
                .HasMany(left => left.PermissionTypes)
                .WithMany(right => right.Roles)
                .UsingEntity(join => join.ToTable("role_permission"));
            //  modelBuilder.ApplyConfiguration(new SeedDataRolePermissions());

            modelBuilder.RemoveOneToManyCascade();

        }

        //needed for Auth
        public virtual DbSet<PermissionType> PermissionTypes { get; set; }
        // public virtual DbSet<RefreshToken> RefreshTokens { get; set; }



    }

    public static class ContextExtensions
    {
        public static void RemoveOneToManyCascade(this ModelBuilder builder)
        {
            builder.EntityLoop(et => et.GetForeignKeys()
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                .ToList()
                .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict));
        }


        private static void EntityLoop(this ModelBuilder builder, Action<IMutableEntityType> action)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                action(entityType);
            }
        }
    }
}

