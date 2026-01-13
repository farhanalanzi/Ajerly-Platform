using Microsoft.AspNetCore.Mvc;
using Ajerly_Platform.Models;
using Ajerly_Platform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

namespace Ajerly_Platform.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public RequestsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Request model, List<IFormFile>? images)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (images != null && images.Any())
            {
                var imageFile = images.First();
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Use same uploads folder as listings for consistency
                    var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "uploads");
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    model.ImagePath = $"/images/uploads/{uniqueFileName}";

                    // Log the saved path to console for debugging
                    Console.WriteLine($"[RequestsController] Saved image to: {filePath}, ImagePath set to: {model.ImagePath}");
                }
            }

            model.CreatedAt = DateTime.Now;
            model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Requests.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // New: Details view for a request
        public async Task<IActionResult> Details(int id)
        {
            var request = await _context.Requests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (request == null)
                return NotFound();

            return View(request);
        }
    }
}