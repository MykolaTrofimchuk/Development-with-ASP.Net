using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ApplicationController : Controller
{
    private const string SessionKey = "ApplicationForm";

    public IActionResult Apply()
    {
        // Якщо є дані в сесії — відновлюємо
        var json = HttpContext.Session.GetString(SessionKey);
        ApplicationForm model = string.IsNullOrEmpty(json)
            ? new ApplicationForm()
            : JsonConvert.DeserializeObject<ApplicationForm>(json);

        return View(model);
    }

    [HttpPost]
    public IActionResult Apply(ApplicationForm model)
    {
        // Зберігаємо поточний стан у сесії
        var json = JsonConvert.SerializeObject(model);
        HttpContext.Session.SetString(SessionKey, json);

        ViewBag.Message = "Заявку збережено в сесії. Можна продовжити пізніше.";
        return View(model);
    }

    public IActionResult Submit()
    {
        // Отримуємо фінальні дані
        var json = HttpContext.Session.GetString(SessionKey);
        if (json == null)
        {
            return RedirectToAction("Apply");
        }

        var model = JsonConvert.DeserializeObject<ApplicationForm>(json);

        // TODO: збереження в базу даних

        // очищаємо сесію після відправки
        HttpContext.Session.Remove(SessionKey);

        ViewBag.Message = "Заявку успішно подано!";
        return View("Result", model);
    }
}
