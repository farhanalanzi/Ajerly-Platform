using Microsoft.AspNetCore.Mvc;
using Ajerly_Platform.Data;
using Ajerly_Platform.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

namespace Ajerly_Platform.Controllers
{
    public class ListingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ListingsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

       
        public async Task<IActionResult> Index(string searchString)
        {
            // preserve the current search so the view can show it
            ViewData["CurrentFilter"] = searchString;

            // start query including images so views can use them
            var query = _context.Listings
                .Include(l => l.Images)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var pattern = $"%{searchString}%";
                query = query.Where(l => EF.Functions.Like(l.Title, pattern) || EF.Functions.Like(l.Description, pattern));
            }

            var listings = await query
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return View(listings);
        }

        // Ajax partial endpoint for live search
        [HttpGet]
        public async Task<IActionResult> IndexPartial(string searchString)
        {
            var query = _context.Listings
                .Include(l => l.Images)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var pattern = $"%{searchString}%";
                query = query.Where(l => EF.Functions.Like(l.Title, pattern) || EF.Functions.Like(l.Description, pattern));
            }

            var listings = await query
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return PartialView("_ListingsGrid", listings);
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

            // Handle image uploads
            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        
                        // Ensure the directory exists - use web root path
                        var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "images", "uploads");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Update filePath to use the ensured directory
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        var listingImage = new ListingImage
                        {
                            ImageUrl = $"/images/uploads/{fileName}",
                            Listing = listing
                        };

                        _context.ListingImages.Add(listingImage);
                    }
                }

                await _context.SaveChangesAsync();
            }

            // After publishing a listing, redirect the user to the site home page
            return RedirectToAction("Index", "Home");
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
                .Include(l => l.User)
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
