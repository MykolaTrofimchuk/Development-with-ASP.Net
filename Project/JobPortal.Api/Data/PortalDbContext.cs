using Microsoft.EntityFrameworkCore;
using JobPortal.Api.Models;
using System.Collections.Generic;

namespace JobPortal.Api.Data
{
    public class PortalDbContext : DbContext
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options) : base(options) { }

        public DbSet<Job> Jobs { get; set; } = null!;
        public DbSet<Application> Applications { get; set; } = null!;
    }
}
