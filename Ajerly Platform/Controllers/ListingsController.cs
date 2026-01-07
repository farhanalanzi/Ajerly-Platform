using Microsoft.AspNetCore.Mvc;
using Ajerly_Platform.Data;
using Ajerly_Platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Ajerly_Platform.Controllers
{
    public class ListingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListingsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            var listings = await _context.Listings.ToListAsync();
            return View(listings);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(Listing listing, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
                return View(listing);

            listing.CreatedAt = DateTime.Now;

            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Category(string name)
        {
            var listings = string.IsNullOrEmpty(name) || name == "الكل"
                ? await _context.Listings.ToListAsync()
                : await _context.Listings.Where(l => l.Category == name).ToListAsync();

            ViewBag.SelectedCategory = name;
            return View("Index", listings);
        }

        // New: Details view for a listing
        public async Task<IActionResult> Details(int id)
        {
            var listing = await _context.Listings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (listing == null)
                return NotFound();

            return View(listing);
        }
    }
}