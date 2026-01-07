using Microsoft.AspNetCore.Mvc;
using Ajerly_Platform.Models;
using Ajerly_Platform.Data;
using Microsoft.EntityFrameworkCore;

namespace Ajerly_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var vm = new HomeIndexViewModel
            {
                Category = "الكل",
                Listings = _context.Listings
                    .Include(l => l.Images)
                    .OrderByDescending(l => l.CreatedAt)
                    .ToList(),

                Requests = _context.Requests
                    .OrderByDescending(r => r.CreatedAt)
                    .ToList()
            };

            return View(vm);
        }
    }
}