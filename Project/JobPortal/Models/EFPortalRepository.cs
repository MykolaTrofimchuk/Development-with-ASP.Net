using Microsoft.EntityFrameworkCore;

namespace JobPortal.Models
{
    public class EFPortalRepository : IPortalRepository
    {
        private PortalDbContext context;
        public EFPortalRepository(PortalDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<User> Users => context.Users;

        public IQueryable<Job> Jobs => context.Jobs;
        public IQueryable<Application> Applications => context.Applications;

        // ===== JOBS =====
        public Job? GetJobById(int id) => context.Jobs.FirstOrDefault(j => j.Id == id);

        public void CreateJob(Job job)
        {
            context.Jobs.Add(job);
            context.SaveChanges();
        }

        public void UpdateJob(Job job)
        {
            context.Jobs.Update(job);
            context.SaveChanges();
        }

        public void DeleteJob(Job job)
        {
            context.Jobs.Remove(job);
            context.SaveChanges();
        }

        // ===== APPLICATIONS =====

        public Application GetApplicationById(int id) =>
            context.Applications.FirstOrDefault(a => a.Id == id);

        public void CreateApplication(Application app)
        {
            context.Applications.Add(app);
            context.SaveChanges();
        }

        public void UpdateApplication(Application app)
        {
            context.Applications.Update(app);
            context.SaveChanges();
        }

        public void DeleteApplication(Application app)
        {
            context.Applications.Remove(app);
            context.SaveChanges();
        }

        public void SaveChanges() => context.SaveChanges();
    }
}
