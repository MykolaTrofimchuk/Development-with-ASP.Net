namespace JobPortal.Api.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string ResumeText { get; set; } = "";
        public DateTime DateApplied { get; set; } = DateTime.UtcNow;

        public int JobId { get; set; }
        public Job? Job { get; set; }
    }
}
