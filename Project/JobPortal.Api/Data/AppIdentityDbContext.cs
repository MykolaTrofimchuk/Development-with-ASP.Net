using Microsoft.EntityFrameworkCore;
using JobPortal.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace JobPortal.Api.Data
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }
    }
}
