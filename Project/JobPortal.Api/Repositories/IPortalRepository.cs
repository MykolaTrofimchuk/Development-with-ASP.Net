using JobPortal.Api.Models;

namespace JobPortal.Api.Repositories
{
    public interface IPortalRepository
    {
        IQueryable<Job> Jobs { get; }
        IQueryable<Application> Applications { get; }

        void CreateJob(Job job);
        void UpdateJob(Job job);
        void DeleteJob(Job job);
        Job? GetJobById(int id);

        void CreateApplication(Application a);
        void UpdateApplication(Application a);
        void DeleteApplication(Application a);
        Application? GetApplicationById(int id);
    }
}
