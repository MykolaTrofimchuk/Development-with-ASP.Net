using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs.Auth
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string FullName { get; set; } = "";

        [Required, MinLength(8)]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = "";

        [Required]
        public string Role { get; set; } = ""; // "Candidate" or "Employer"
    }
}
