using Microsoft.AspNetCore.Mvc;
using Ajerly_Platform.Data;
using Ajerly_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Listing listing, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
                return View(listing);

            listing.CreatedAt = DateTime.Now;
            // set owner
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            listing.UserId = userId;

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

        [Authorize]
        public async Task<IActionResult> MyAds()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var myListings = await _context.Listings
                .Where(l => l.UserId == userId)
                .Include(l => l.Images)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return View(myListings);
        }

        // Edit GET
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (listing.UserId != userId)
                return Forbid();

            return View(listing);
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Listing model)
        {
            if (id != model.Id)
                return BadRequest();

            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (listing.UserId != userId)
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            // update allowed fields
            listing.Title = model.Title;
            listing.Category = model.Category;
            listing.Description = model.Description;
            listing.Price = model.Price;
            listing.PriceUnit = model.PriceUnit;
            listing.City = model.City;
            listing.Phone = model.Phone;

            _context.Listings.Update(listing);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyAds");
        }

        // Delete GET (confirmation)
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (listing.UserId != userId)
                return Forbid();

            return View(listing);
        }

        // Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listing = await _context.Listings.FindAsync(id);
            if (listing == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (listing.UserId != userId)
                return Forbid();

            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyAds");
        }
    }
}
