using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models
{
    public class APIDbContext:DbContext
    {
        public APIDbContext(DbContextOptions options):base(options) 
        {

        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Client> Clients { get; set; }

    }
}
