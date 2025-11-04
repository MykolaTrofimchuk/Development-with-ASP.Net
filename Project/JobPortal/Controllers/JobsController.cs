using JobPortal.Models;
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

        // READ: список
        public IActionResult Index() => View(_repository.Jobs);

        // READ: деталі
        public IActionResult Details(int id)
        {
            var job = _repository.GetJobById(id);
            if (job == null) return NotFound();
            return View(job);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View("Edit", new Job());
        }

        // CREATE (POST)
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

        // UPDATE (GET)
        public IActionResult Edit(int id)
        {
            var job = _repository.GetJobById(id);
            if (job == null) return NotFound();
            return View(job);
        }

        // UPDATE (POST)
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

        // DELETE (GET) — підтвердження
        public IActionResult Delete(int id)
        {
            var job = _repository.GetJobById(id);
            if (job == null) return NotFound();
            return View(job);
        }

        // DELETE (POST)
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
