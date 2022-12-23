using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Net6StarterApp.Models;

namespace Net6StarterApp.Data
{
	public class MyDbContext : IdentityDbContext<MyUser>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<SampleObj> SampleObjs => Set<SampleObj>();
    }
}

