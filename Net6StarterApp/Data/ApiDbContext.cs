using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Net6StarterApp.Authentication.Models;
using Net6StarterApp.Authentication.Data.SeedData;
using Net6StarterApp.Models;
using Net6StarterApp.Authentication.Permissions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Net6StarterApp.Authentication.Data;

namespace Net6StarterApp.Data
{


    public class ApiDbContext : ApiAuthDbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
        }

        //Add DbSets here
        
        //public virtual DbSet<SampleObj> SampleObjs { get; set; }






       
    }

    

}

