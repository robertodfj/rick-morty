using Microsoft.EntityFrameworkCore;

namespace RickYMorty.data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        
        public DbSet<Character> Characters { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<User> Users { get; set; }
    }

}