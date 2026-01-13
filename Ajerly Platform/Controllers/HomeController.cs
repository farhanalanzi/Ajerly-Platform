using Microsoft.AspNetCore.Mvc;
using Ajerly_Platform.Models;
using Ajerly_Platform.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System;

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
            // Move Union before the final projection and ensure only server-translatable
            // expressions are used in the queries. Use FirstOrDefault() on a sub-select
            // to get the first image URL which EF Core can translate.
            var listings = _context.Listings.Select(l => new
            {
                l.Id,
                l.Title,
                l.CreatedAt,
                Type = "Listing",
                ImageUrl = l.Images.OrderBy(i => i.Id).Select(i => i.ImageUrl).FirstOrDefault(),
                Category = l.Category,
                Description = l.Description,
                City = l.City,
                Price = l.Price,
                PriceUnit = l.PriceUnit,
                Phone = l.Phone,
                SellerName = l.User.FullName // may be null
            });

            var requests = _context.Requests.Select(r => new
            {
                r.Id,
                r.Title,
                r.CreatedAt,
                Type = "Request",
                ImageUrl = r.ImagePath,
                Category = r.Category,
                Description = r.Description,
                City = r.City,
                Price = r.Price,
                PriceUnit = r.PriceUnit,
                Phone = r.Phone,
                SellerName = r.User.FullName
            });

            // Perform the set operation on the IQueryable results, then order and
            // materialize the results, and finally project into the view model type.
            var combinedItems = listings
                .Union(requests)
                .OrderByDescending(i => i.CreatedAt)
                .ToList() // materialize from database
                .Select(i => new CombinedItem
                {
                    Id = i.Id,
                    Title = i.Title,
                    CreatedAt = i.CreatedAt,
                    Type = i.Type,
                    ImageUrl = i.ImageUrl,
                    Category = i.Category,
                    Description = i.Description,
                    City = i.City,
                    Price = i.Price,
                    PriceUnit = i.PriceUnit,
                    Phone = i.Phone,
                    SellerName = i.SellerName // map seller name
                })
                .ToList();

            // Debug: log how many combined items and how many requests lack ImageUrl
            Console.WriteLine($"[HomeController] CombinedItems: {combinedItems.Count}");
            var requestsWithoutImage = combinedItems.Count(ci => ci.Type == "Request" && string.IsNullOrEmpty(ci.ImageUrl));
            Console.WriteLine($"[HomeController] Requests without image: {requestsWithoutImage}");

            var vm = new HomeIndexViewModel
            {
                Category = "الكل",
                CombinedItems = combinedItems
            };

            return View(vm);
        }

        // Debug endpoint: returns minimal combined items (Id, Type, ImageUrl) as JSON
        [ResponseCache(NoStore = true, Duration = 0)]
        public IActionResult DebugCombined()
        {
            var listings = _context.Listings.Select(l => new
            {
                Id = l.Id,
                Type = "Listing",
                ImageUrl = l.Images.OrderBy(i => i.Id).Select(i => i.ImageUrl).FirstOrDefault()
            });

            var requests = _context.Requests.Select(r => new
            {
                Id = r.Id,
                Type = "Request",
                ImageUrl = r.ImagePath
            });

            var combined = listings.Union(requests).OrderByDescending(x => x.Id).ToList();

            return Json(combined.Select(x => new { x.Id, x.Type, x.ImageUrl }));
        }
    }
}
