namespace JobPortal.Models
{
    public interface IPortalRepository
    {
        IQueryable<User> Users { get; }

        IQueryable<Job> Jobs { get; }
        Job? GetJobById(int id);
        void CreateJob(Job job);
        void UpdateJob(Job job);
        void DeleteJob(Job job);

        IQueryable<Application> Applications { get; }
        Application GetApplicationById(int id);
        void CreateApplication(Application app);
        void UpdateApplication(Application app);
        void DeleteApplication(Application app);

        void SaveChanges();
    }
}
