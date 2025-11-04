using JobPortal.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class Application
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Вкажіть ім’я заявника")]
        [StringLength(100, ErrorMessage = "Ім’я не може перевищувати 100 символів")]
        [Display(Name = "Ім’я заявника")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Вкажіть адресу електронної пошти")]
        [EmailAddress(ErrorMessage = "Невірний формат електронної пошти")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage = "Коментар не може перевищувати 500 символів")]
        [Display(Name = "Коментар / супровідний лист")]
        public string ResumeText { get; set; }

        public DateTime DateApplied { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Оберіть вакансію")]
        [Display(Name = "Вакансія")]
        public int JobId { get; set; }

        [ValidateNever]
        public Job Job { get; set; }
    }
}