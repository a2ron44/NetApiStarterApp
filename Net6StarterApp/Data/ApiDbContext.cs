﻿using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Net6StarterApp.Authentication.Models;
using Net6StarterApp.Authentication.Data.SeedData;
using Net6StarterApp.Models;

namespace Net6StarterApp.Data
{


    public class ApiDbContext : IdentityDbContext<ApiUser>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed data for auth
            modelBuilder.ApplyConfiguration(new SeedDataRoles());


        }


        public DbSet<SampleObj> SampleObjs => Set<SampleObj>();


        //needed for Auth
       // public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
