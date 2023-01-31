using Microsoft.EntityFrameworkCore;
using NetApiStarterLibrary.Data;
using NetApiStarterLibrary.Data.SeedData;

namespace NetApiStarterApp.Data
{


    public class ApiDbContext : ApiAuthDbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed data for Roles/Permissions using NetApiStarterLibrary.  Can modify/extend as needed
            modelBuilder.ApplyConfiguration(new SeedDataRoles());
            modelBuilder.ApplyConfiguration(new SeedDataPermissions());
        }

        //Add DbSets here
        
        //public virtual DbSet<SampleObj> SampleObjs { get; set; }






       
    }

    

}

