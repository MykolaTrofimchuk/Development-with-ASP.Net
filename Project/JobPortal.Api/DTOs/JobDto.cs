using System.ComponentModel.DataAnnotations;

namespace JobPortal.Api.DTOs
{
    public class JobDto
    {
        [Required]
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";
        public string Company { get; set; } = "";
    }
}
