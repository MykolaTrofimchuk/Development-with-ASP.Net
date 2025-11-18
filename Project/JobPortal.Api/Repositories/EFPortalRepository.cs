using JobPortal.Api.Data;
using JobPortal.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Api.Repositories
{
    public class EFPortalRepository : IPortalRepository
    {
        private readonly PortalDbContext _ctx;
        public EFPortalRepository(PortalDbContext ctx) { _ctx = ctx; }

        public IQueryable<Job> Jobs => _ctx.Jobs;
        public IQueryable<Application> Applications => _ctx.Applications;

        public void CreateJob(Job job) { _ctx.Jobs.Add(job); _ctx.SaveChanges(); }
        public void UpdateJob(Job job) { _ctx.Jobs.Update(job); _ctx.SaveChanges(); }
        public void DeleteJob(Job job) { _ctx.Jobs.Remove(job); _ctx.SaveChanges(); }
        public Job? GetJobById(int id) => _ctx.Jobs.Include(j => j.Applications).FirstOrDefault(j => j.Id == id);

        public void CreateApplication(Application a) { _ctx.Applications.Add(a); _ctx.SaveChanges(); }
        public void UpdateApplication(Application a) { _ctx.Applications.Update(a); _ctx.SaveChanges(); }
        public void DeleteApplication(Application a) { _ctx.Applications.Remove(a); _ctx.SaveChanges(); }
        public Application? GetApplicationById(int id) => _ctx.Applications.Include(a => a.Job).FirstOrDefault(a => a.Id == id);
    }
}
