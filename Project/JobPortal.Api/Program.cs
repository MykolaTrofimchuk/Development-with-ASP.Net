using JobPortal.Api.Data;
using JobPortal.Api.Models;
using JobPortal.Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // DbContexts
        builder.Services.AddDbContext<PortalDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("JobPortalConnection")));

        builder.Services.AddDbContext<AppIdentityDbContext>(opts =>
            opts.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

        // Identity
        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
        })
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();

        // Jwt authentication
        var jwt = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwt["Issuer"],
                ValidAudience = jwt["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        builder.Services.AddScoped<IPortalRepository, EFPortalRepository>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var identityContext = services.GetRequiredService<AppIdentityDbContext>();
            var portalContext = services.GetRequiredService<PortalDbContext>();

            try
            {
                // AUTOMATICALLY APPLY MIGRATIONS / CREATE DB
                await identityContext.Database.MigrateAsync();
                await portalContext.Database.MigrateAsync();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "Candidate", "Employer" };
                foreach (var r in roles)
                {
                    if (!await roleManager.RoleExistsAsync(r))
                    {
                        await roleManager.CreateAsync(new IdentityRole(r));
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}