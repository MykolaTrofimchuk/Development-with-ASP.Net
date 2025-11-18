using JobPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class JobsController : Controller
    {
        private readonly IPortalRepository _repository;

        public JobsController(IPortalRepository repo)
        {
            _repository = repo;
        }

        // ---------- PUBLIC (доступні всім) ----------
        [AllowAnonymous]
        public IActionResult Index() => View(_repository.Jobs);

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var job = _repository.GetJobById(id);
            if (job == null) return NotFound();
            return View(job);
        }

        // ---------- EMPLOYER ONLY ----------
        [Authorize(Roles = "Employer")]
        public IActionResult Create()
        {
            return View("Edit", new Job());
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Job job)
        {
            if (ModelState.IsValid)
            {
                _repository.CreateJob(job);
                return RedirectToAction("Index");
            }
            return View("Edit", job);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult Edit(int id)
        {
            var job = _repository.GetJobById(id);
            if (job == null) return NotFound();
            return View(job);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Job job)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateJob(job);
                return RedirectToAction("Index");
            }
            return View(job);
        }

        [Authorize(Roles = "Employer")]
        public IActionResult Delete(int id)
        {
            var job = _repository.GetJobById(id);
            if (job == null) return NotFound();
            return View(job);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var job = _repository.GetJobById(id);
            _repository.DeleteJob(job);
            return RedirectToAction("Index");
        }
    }

}
