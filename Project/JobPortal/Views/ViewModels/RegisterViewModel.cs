using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string FullName { get; set; } = "";

        [Required, DataType(DataType.Password)]
        [MinLength(8)]
        public string Password { get; set; } = "";

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = "";

		[Required]
		public string Role { get; set; } = "";
	}
}
