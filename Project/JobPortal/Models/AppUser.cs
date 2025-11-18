using Microsoft.AspNetCore.Identity;

namespace JobPortal.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Candidate / Employer
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
