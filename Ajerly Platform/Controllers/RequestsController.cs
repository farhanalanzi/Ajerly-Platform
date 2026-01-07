using Microsoft.AspNetCore.Mvc;
using Ajerly_Platform.Models;
using Ajerly_Platform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Ajerly_Platform.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Request model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.CreatedAt = DateTime.Now;
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Requests.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // New: Details view for a request
        public async Task<IActionResult> Details(int id)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
                return NotFound();

            return View(request);
        }
    }
}