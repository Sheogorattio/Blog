using Blog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        public DbSet<Membership> Memberships { get; set; }
    }
}
