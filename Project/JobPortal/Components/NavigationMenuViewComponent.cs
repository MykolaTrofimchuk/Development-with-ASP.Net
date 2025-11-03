using Microsoft.AspNetCore.Mvc;
using JobPortal.Models;
using System.Linq;

namespace JobPortal.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly PortalDbContext _context;

        public NavigationMenuViewComponent(PortalDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            // Отримуємо унікальні ролі користувачів
            var roles = _context.Users
                .Select(u => u.Role)
                .Distinct()
                .OrderBy(r => r)
                .ToList();

            // Зчитуємо обрану роль з query string
            string? selectedRole = HttpContext.Request.Query["role"];

            ViewBag.SelectedRole = selectedRole;

            return View(roles);
        }
    }
}
