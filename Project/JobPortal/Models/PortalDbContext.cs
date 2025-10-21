using Microsoft.EntityFrameworkCore;

namespace JobPortal.Models
{
    public class PortalDbContext : DbContext
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
    }
}
