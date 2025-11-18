using Microsoft.AspNetCore.Mvc;
using JobPortal.Api.Repositories;
using JobPortal.Api.Models;
using JobPortal.Api.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IPortalRepository _repo;
        public JobsController(IPortalRepository repo) { _repo = repo; }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repo.Jobs.ToList());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var job = _repo.GetJobById(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public IActionResult Create(JobDto dto)
        {
            var job = new Job { Title = dto.Title, Company = dto.Company, Description = dto.Description };
            _repo.CreateJob(job);
            return CreatedAtAction(nameof(Get), new { id = job.Id }, job);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employer")]
        public IActionResult Update(int id, JobDto dto)
        {
            var existing = _repo.GetJobById(id);
            if (existing == null) return NotFound();

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Company = dto.Company;
            _repo.UpdateJob(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employer")]
        public IActionResult Delete(int id)
        {
            var job = _repo.GetJobById(id);
            if (job == null) return NotFound();
            _repo.DeleteJob(job);
            return NoContent();
        }
    }
}
