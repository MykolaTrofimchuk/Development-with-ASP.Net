using Microsoft.AspNetCore.Mvc;

namespace MvcLabProject.Controllers
{
    public class LabController : Controller
    {
        public IActionResult Info()
        {
            var labData = new
            {
                Number = 1,
                Topic = "Вступ до ASP.NET Core",
                Purpose = "ознайомитися з основними принципами роботи .NET, навчитися налаштовувати середовище розробки та встановлювати необхідні компоненти, " +
                "набути навичок створення рішень та проектів різних типів, набути навичок обробки запитів з використанням middleware",
                Author = "Микола Трофімчук ІПЗ-22-2"
            };

            // Передаємо дані через ViewData
            ViewData["Number"] = labData.Number;
            ViewData["Topic"] = labData.Topic;
            ViewData["Purpose"] = labData.Purpose;
            ViewData["Author"] = labData.Author;

            return View();
        }
    }
}
