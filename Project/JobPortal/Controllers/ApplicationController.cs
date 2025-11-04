using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class ApplicationController : Controller
{
    private readonly IPortalRepository _repository;

    public ApplicationController(IPortalRepository repo)
    {
        _repository = repo;
    }

    // READ: список
    public async Task<IActionResult> Index()
    {
        var applications = _repository.Applications
            .Include(a => a.Job); // 
        return View(await applications.ToListAsync());
    }

    // READ: деталі
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var application = await _repository.Applications
            .Include(a => a.Job) 
            .FirstOrDefaultAsync(m => m.Id == id);

        if (application == null)
        {
            return NotFound();
        }

        return View(application);
    }


    // CREATE (GET)
    public IActionResult Create()
    {
        ViewBag.Jobs = _repository.Jobs.ToList();
        return View("Edit", new Application());
    }

    // CREATE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Application app)
    {
        if (ModelState.IsValid)
        {
            _repository.CreateApplication(app);
            return RedirectToAction("Index");
        }
        ViewBag.Jobs = _repository.Jobs.ToList();
        return View("Edit", app);
    }

    // UPDATE (GET)
    public IActionResult Edit(int id)
    {
        var app = _repository.GetApplicationById(id);
        if (app == null) return NotFound();
        ViewBag.Jobs = _repository.Jobs.ToList();
        return View(app);
    }

    // UPDATE (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Application app)
    {
        if (ModelState.IsValid)
        {
            _repository.UpdateApplication(app);
            return RedirectToAction("Index");
        }
        ViewBag.Jobs = _repository.Jobs.ToList();
        return View(app);
    }

    // DELETE (GET)
    public IActionResult Delete(int id)
    {
        var app = _repository.Applications
            .Include(a => a.Job)
            .FirstOrDefault(a => a.Id == id);

        if (app == null) return NotFound();

        return View(app);
    }

    // DELETE (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var app = _repository.GetApplicationById(id);
        if (app != null)
        {
            _repository.DeleteApplication(app);
        }
        return RedirectToAction("Index");
    }


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

        // очищаємо сесію після відправки
        HttpContext.Session.Remove(SessionKey);

        ViewBag.Message = "Заявку успішно подано!";
        return View("Result", model);
    }
}
