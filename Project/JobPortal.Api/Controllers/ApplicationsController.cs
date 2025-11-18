using Microsoft.AspNetCore.Mvc;
using JobPortal.Api.Repositories;
using JobPortal.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IPortalRepository _repo;
        public ApplicationsController(IPortalRepository repo) { _repo = repo; }

        [HttpGet]
        [Authorize(Roles = "Employer")]
        public IActionResult GetAll() => Ok(_repo.Applications.ToList());

        [HttpGet("{id}")]
        [Authorize(Roles = "Employer")]
        public IActionResult Get(int id)
        {
            var app = _repo.GetApplicationById(id);
            if (app == null) return NotFound();
            return Ok(app);
        }

        [HttpPost]
        [Authorize(Roles = "Candidate")]
        public IActionResult Create(Application app)
        {
            _repo.CreateApplication(app);
            return CreatedAtAction(nameof(Get), new { id = app.Id }, app);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employer")]
        public IActionResult Delete(int id)
        {
            var app = _repo.GetApplicationById(id);
            if (app == null) return NotFound();
            _repo.DeleteApplication(app);
            return NoContent();
        }
    }
}
