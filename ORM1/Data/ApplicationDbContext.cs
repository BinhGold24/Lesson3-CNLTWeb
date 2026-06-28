using Microsoft.EntityFrameworkCore;
using Lesson3_CNLTWeb.Models;

namespace Lesson3_CNLTWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}