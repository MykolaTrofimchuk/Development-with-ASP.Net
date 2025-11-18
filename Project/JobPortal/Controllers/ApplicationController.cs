using JobPortal.Models;
using Microsoft.AspNetCore.Authorization;
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

    // ======= EMPLOYER AREA =======

    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> Index()
    {
        var applications = _repository.Applications.Include(a => a.Job);
        return View(await applications.ToListAsync());
    }

    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var application = await _repository.Applications
            .Include(a => a.Job)
            .FirstOrDefaultAsync(m => m.Id == id);

        return application == null ? NotFound() : View(application);
    }

    // EMPLOYER ONLY CRUD
    [Authorize(Roles = "Employer")]
    public IActionResult Create()
    {
        ViewBag.Jobs = _repository.Jobs.ToList();
        return View("Edit", new Application());
    }

    [Authorize(Roles = "Employer")]
    [HttpPost, ValidateAntiForgeryToken]
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

    [Authorize(Roles = "Employer")]
    public IActionResult Edit(int id)
    {
        var app = _repository.GetApplicationById(id);
        if (app == null) return NotFound();
        ViewBag.Jobs = _repository.Jobs.ToList();
        return View(app);
    }

    [Authorize(Roles = "Employer")]
    [HttpPost, ValidateAntiForgeryToken]
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

    [Authorize(Roles = "Employer")]
    public IActionResult Delete(int id)
    {
        var app = _repository.Applications
            .Include(a => a.Job)
            .FirstOrDefault(a => a.Id == id);

        return app == null ? NotFound() : View(app);
    }

    [Authorize(Roles = "Employer")]
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var app = _repository.GetApplicationById(id);
        if (app != null) _repository.DeleteApplication(app);
        return RedirectToAction("Index");
    }


    // ======= CANDIDATE ZONE =======

    [Authorize(Roles = "Candidate")]
    public IActionResult Apply()
    {
        var json = HttpContext.Session.GetString(SessionKey);
        ApplicationForm model = string.IsNullOrEmpty(json)
            ? new ApplicationForm()
            : JsonConvert.DeserializeObject<ApplicationForm>(json);

        return View(model);
    }

    [Authorize(Roles = "Candidate")]
    [HttpPost]
    public IActionResult Apply(ApplicationForm model)
    {
        HttpContext.Session.SetString(SessionKey, JsonConvert.SerializeObject(model));
        ViewBag.Message = "Заявку збережено в сесії. Можна продовжити пізніше.";
        return View(model);
    }

    [Authorize(Roles = "Candidate")]
    public IActionResult Submit()
    {
        var json = HttpContext.Session.GetString(SessionKey);
        if (json == null)
            return RedirectToAction("Apply");

        var model = JsonConvert.DeserializeObject<ApplicationForm>(json);
        HttpContext.Session.Remove(SessionKey);

        ViewBag.Message = "Заявку успішно подано!";
        return View("Result", model);
    }

    private const string SessionKey = "ApplicationForm";
}
