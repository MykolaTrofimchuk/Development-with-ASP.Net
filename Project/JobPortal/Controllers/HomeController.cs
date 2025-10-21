using Microsoft.AspNetCore.Mvc;
using JobPortal.Models;
using JobPortal.Models.ViewModels;
using JobPortal.Views.ViewModels;

namespace JobPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly PortalDbContext _context;
        private const int PageSize = 3; // кількість користувачів на сторінці

        public HomeController(PortalDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            var usersQuery = _context.Users.OrderBy(u => u.Id);

            var model = new UsersListViewModel
            {
                Users = usersQuery
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = usersQuery.Count()
                }
            };

            return View(model);
        }
    }
}
