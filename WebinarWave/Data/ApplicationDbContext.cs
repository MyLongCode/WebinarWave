using Microsoft.EntityFrameworkCore;

namespace WebinarWave.Data
{
    public class ApplicationDbContext : DbContext
    {

        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
