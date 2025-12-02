namespace BlazorApp_JobPortal.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
