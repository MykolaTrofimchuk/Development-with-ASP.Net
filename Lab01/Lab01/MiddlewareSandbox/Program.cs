using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// --- Middleware c: Логування всіх запитів ---
app.Use(async (context, next) =>
{
    Console.WriteLine($"[LOG] Method: {context.Request.Method}, Path: {context.Request.Path}");
    await next();
});

// --- Middleware b: Перевірка параметра query string "custom" ---
app.Use(async (context, next) =>
{
    if (context.Request.Query.ContainsKey("custom"))
    {
        await context.Response.WriteAsync("You’ve hit a custom middleware!");
    }
    else
    {
        await next();
    }
});

// --- Middleware d: Перевірка API ключа ---
const string apiKey = "12345"; // Ключ на сервері
app.Use(async (context, next) =>
{
    if (!context.Request.Headers.TryGetValue("X-API-KEY", out var extractedApiKey) || extractedApiKey != apiKey)
    {
        context.Response.StatusCode = 403; // Forbidden
        await context.Response.WriteAsync("Forbidden: invalid or missing API key.");
        return;
    }
    await next();
});

// --- Middleware a: Лічильник запитів ---
int requestCount = 0;
app.Use(async (context, next) =>
{
    requestCount++;
    await next();
});

// --- Простий GET ендпоінт для тесту ---
app.MapGet("/test", async context =>
{
    await context.Response.WriteAsync($"The amount of processed requests is {requestCount}");
});

app.Run();
