using Microsoft.EntityFrameworkCore;
using webb.Core.Domain;

namespace webb.Core.Persistance
{
    public class WebAppDbContext : DbContext
    {

        public WebAppDbContext(DbContextOptions<WebAppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
