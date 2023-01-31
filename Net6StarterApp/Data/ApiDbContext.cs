using Microsoft.EntityFrameworkCore;
using NetApiStarterLibrary.Data;

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

           
        }

        //Add DbSets here
        
        //public virtual DbSet<SampleObj> SampleObjs { get; set; }






       
    }

    

}

