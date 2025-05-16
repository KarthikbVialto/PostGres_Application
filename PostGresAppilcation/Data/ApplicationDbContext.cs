using Microsoft.EntityFrameworkCore;
using PostGresAppilcation.Models;

namespace PostGresAppilcation.Data
{

    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
