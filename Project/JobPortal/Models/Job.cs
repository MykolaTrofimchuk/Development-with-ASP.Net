using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Вкажіть назву вакансії")]
        [StringLength(100, ErrorMessage = "Назва не може перевищувати 100 символів")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Додайте опис вакансії")]
        [StringLength(1000, ErrorMessage = "Опис занадто довгий (до 1000 символів)")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Вкажіть назву компанії")]
        [StringLength(100, ErrorMessage = "Назва компанії не може перевищувати 100 символів")]
        public string Company { get; set; }

        [Display(Name = "Дата публікації")]
        public DateTime DatePosted { get; set; } = DateTime.Now;

        // One-to-many
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
