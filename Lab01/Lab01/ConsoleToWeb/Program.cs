using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// --- GET /who ---
app.MapGet("/who", () => new
{
    Name = "Микола",
    Surname = "Трофімчук"
});

// --- GET /time ---
app.MapGet("/time", () => new
{
    ServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
});

app.Run();
