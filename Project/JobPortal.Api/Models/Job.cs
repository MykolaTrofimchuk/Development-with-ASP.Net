namespace JobPortal.Api.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Company { get; set; } = "";
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;

        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
