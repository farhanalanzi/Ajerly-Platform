using Microsoft.AspNetCore.Mvc;
using Ajerly_Platform.Models;
using Ajerly_Platform.Data;

namespace Ajerly_Platform.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Request model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.CreatedAt = DateTime.Now;

            _context.Requests.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}