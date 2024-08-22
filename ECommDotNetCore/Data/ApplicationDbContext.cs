using ECommDotNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommDotNetCore.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> user { get; set; }
    }
}
