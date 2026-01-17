using Microsoft.EntityFrameworkCore;
using Ajerly_Platform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ajerly_Platform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Listing> Listings { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }
        public DbSet<Request> Requests { get; set; }
    }
}
