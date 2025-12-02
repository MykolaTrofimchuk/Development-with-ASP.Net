using System.ComponentModel.DataAnnotations;

namespace JobPortal.BlazorApp.Models
{
    public class JobDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введіть назву вакансії")]
        [MinLength(3)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Введіть опис")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Введіть назву компанії")]
        public string Company { get; set; }
    }
}
